// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
using AdaptiveCards.Rendering.Uwp;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using AdaptiveCardVisualizer.Helpers;
using AdaptiveCardVisualizer.ResourceResolvers;
using Windows.UI.Xaml.Media;

namespace AdaptiveCardVisualizer.ViewModel
{
    public class DocumentViewModel : GenericDocumentViewModel
    {
        private static AdaptiveCardRenderer _renderer;

        private DocumentViewModel(MainPageViewModel mainPageViewModel) : base(mainPageViewModel) { }

        private RenderedAdaptiveCard _renderedAdaptiveCard;
        private UIElement _renderedCard;
        public UIElement RenderedCard
        {
            get { return _renderedCard; }
            private set { SetProperty(ref _renderedCard, value); }
        }

        public static async Task<DocumentViewModel> LoadFromFileAsync(MainPageViewModel mainPageViewModel, IStorageFile file, string token)
        {
            var answer = new DocumentViewModel(mainPageViewModel);
            await answer.LoadFromFileAsync(file, token, assignPayloadWithoutLoading: true);
            return answer;
        }

        public static DocumentViewModel LoadFromPayload(MainPageViewModel mainPageViewModel, string payload)
        {
            return new DocumentViewModel(mainPageViewModel)
            {
                _payload = payload
            };
        }

        protected override async void LoadPayload(string payload)
        {
            var newErrors = await PayloadValidator.ValidateAsync(payload);
            if (newErrors.Any(i => i.Type == ErrorViewModelType.Error))
            {
                MakeErrorsLike(newErrors);
                return;
            }

            try
            {
                if (_renderer == null)
                {
                    InitializeRenderer(MainPageViewModel.HostConfigEditor.HostConfig);
                }
            }
            catch (Exception ex)
            {
                newErrors.Add(new ErrorViewModel()
                {
                    Message = "Initializing renderer error: " + ex.ToString(),
                    Type = ErrorViewModelType.Error
                });
                MakeErrorsLike(newErrors);
                return;
            }

            try
            {
                JsonObject jsonObject;
                if (JsonObject.TryParse(payload, out jsonObject))
                {
                    AdaptiveElementParserRegistration parserRegistration = new AdaptiveElementParserRegistration();
                    parserRegistration.Set("AnimalGrid", new TestLibrary.AnimalGridParser());
                    parserRegistration.Set("Border", new TestLibrary.BorderCustomElementParser());

                    AdaptiveCardParseResult parseResult = AdaptiveCard.FromJson(jsonObject, parserRegistration, null);

                    _renderedAdaptiveCard = _renderer.RenderAdaptiveCard(parseResult.AdaptiveCard);
                    if (_renderedAdaptiveCard.FrameworkElement != null)
                    {
                        RenderedCard = _renderedAdaptiveCard.FrameworkElement;
                        _renderedAdaptiveCard.Action += async (sender, e) =>
                        {
                            var m_actionDialog = new ContentDialog();

                            if (e.Action.ActionType == ActionType.ShowCard)
                            {
                                AdaptiveShowCardAction showCardAction = (AdaptiveShowCardAction)e.Action;
                                RenderedAdaptiveCard renderedShowCard = _renderer.RenderAdaptiveCard(showCardAction.Card);
                                if (renderedShowCard.FrameworkElement != null)
                                {
                                    m_actionDialog.Content = renderedShowCard.FrameworkElement;
                                }
                            }
                            else
                            {
                                m_actionDialog.Content = SerializeActionEventArgsToString(e);
                            }

                            m_actionDialog.PrimaryButtonText = "Close";

                            await m_actionDialog.ShowAsync();
                        };

                        if (!MainPageViewModel.HostConfigEditor.HostConfig.Media.AllowInlinePlayback)
                        {
                            _renderedAdaptiveCard.MediaClicked += async (sender, e) =>
                            {
                                var onPlayDialog = new ContentDialog();
                                onPlayDialog.Content = "MediaClickedEvent:";

                                foreach (var source in e.Media.Sources)
                                {
                                    onPlayDialog.Content += "\n" + source.Url + " (" + source.MimeType + ")";
                                }

                                onPlayDialog.PrimaryButtonText = "Close";

                                await onPlayDialog.ShowAsync();
                            };
                        }
                    }
                    else
                    {
                        newErrors.Add(new ErrorViewModel()
                        {
                            Message = "There was an error Rendering this card",
                            Type = ErrorViewModelType.Error
                        });
                    }
                    foreach (var error in parseResult.Errors)
                    {
                        newErrors.Add(new ErrorViewModel()
                        {
                            Message = error.Message,
                            Type = ErrorViewModelType.Error
                        });
                    }
                    foreach (var error in _renderedAdaptiveCard.Errors)
                    {
                        newErrors.Add(new ErrorViewModel()
                        {
                            Message = error.Message,
                            Type = ErrorViewModelType.Error
                        });
                    }
                    foreach (var error in parseResult.Warnings)
                    {
                        newErrors.Add(new ErrorViewModel()
                        {
                            Message = error.Message,
                            Type = ErrorViewModelType.Warning
                        });
                    }

                    foreach (var error in _renderedAdaptiveCard.Warnings)
                    {
                        newErrors.Add(new ErrorViewModel()
                        {
                            Message = error.Message,
                            Type = ErrorViewModelType.Warning
                        });
                    }
                }
                else
                {
                    newErrors.Add(new ErrorViewModel()
                    {
                        Message = "There was an error creating a JsonObject from the card",
                        Type = ErrorViewModelType.Error
                    });
                }

                if (RenderedCard is FrameworkElement)
                {
                    (RenderedCard as FrameworkElement).VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top;
                }
                MakeErrorsLike(newErrors);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                newErrors.Add(new ErrorViewModel()
                {
                    Message = "Rendering failed (" + ex.Message + ")",
                    Type = ErrorViewModelType.Error
                });
                MakeErrorsLike(newErrors);
            }
        }

        public string SerializeActionEventArgsToString(AdaptiveActionEventArgs args)
        {
            string answer = "Action invoked!";

            answer += "\nType: " + args.Action.ActionType;

            if (args.Action is AdaptiveSubmitAction)
            {
                answer += "\nData: " + (args.Action as AdaptiveSubmitAction).DataJson?.Stringify();
            }
            else if (args.Action is AdaptiveOpenUrlAction)
            {
                answer += "\nUrl: " + (args.Action as AdaptiveOpenUrlAction).Url;
            }

            answer += "\nInputs: " + args.Inputs.AsJson().Stringify();

            return answer;
        }

        public static void InitializeRenderer(AdaptiveHostConfig hostConfig)
        {
            _renderer = new AdaptiveCardRenderer();
            if (hostConfig != null)
            {
                _renderer.HostConfig = hostConfig;
            }

            // Add a feature representing this version of the visualizer. used for test cards.
            _renderer.FeatureRegistration.Set("acTest", "1.0");

            if (Settings.UseFixedDimensions)
            {
                _renderer.SetFixedDimensions(320, 180);
            }

            _renderer.ElementRenderers.Set("AnimalGrid", new TestLibrary.AnimalGridRenderer());
            _renderer.ElementRenderers.Set("Border", new TestLibrary.BorderCustomElementRenderer());

            // Custom resource resolvers
            _renderer.ResourceResolvers.Set("symbol", new MySymbolResourceResolver());

            /*
                *Example on how to override the Action Positive and Destructive styles
            Style positiveStyle = new Style(typeof(Button));
            positiveStyle.Setters.Add(new Setter(Button.BackgroundProperty, new SolidColorBrush(Windows.UI.Colors.LawnGreen)));
            Style destructiveStyle = new Style(typeof(Button));
            destructiveStyle.Setters.Add(new Setter(Button.BackgroundProperty, new SolidColorBrush(Windows.UI.Colors.Red)));
            Style otherStyle = new Style(typeof(Button));
            otherStyle.Setters.Add(new Setter(Button.BackgroundProperty, new SolidColorBrush(Windows.UI.Colors.Yellow)));
            otherStyle.Setters.Add(new Setter(Button.ForegroundProperty, new SolidColorBrush(Windows.UI.Colors.DarkRed)));

            _renderer.OverrideStyles = new ResourceDictionary();
            _renderer.OverrideStyles.Add("Adaptive.Action.Positive", positiveStyle);
            _renderer.OverrideStyles.Add("Adaptive.Action.Destructive", destructiveStyle);
            _renderer.OverrideStyles.Add("Adaptive.Action.other", otherStyle);
            */
        }
    }
}
