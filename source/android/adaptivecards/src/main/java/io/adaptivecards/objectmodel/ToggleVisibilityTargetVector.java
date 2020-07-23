/* ----------------------------------------------------------------------------
 * This file was automatically generated by SWIG (http://www.swig.org).
 * Version 4.0.1
 *
 * Do not make changes to this file unless you know what you are doing--modify
 * the SWIG interface file instead.
 * ----------------------------------------------------------------------------- */

package io.adaptivecards.objectmodel;

public class ToggleVisibilityTargetVector extends java.util.AbstractList<ToggleVisibilityTarget> implements java.util.RandomAccess {
  private transient long swigCPtr;
  protected transient boolean swigCMemOwn;

  protected ToggleVisibilityTargetVector(long cPtr, boolean cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = cPtr;
  }

  protected static long getCPtr(ToggleVisibilityTargetVector obj) {
    return (obj == null) ? 0 : obj.swigCPtr;
  }

  @SuppressWarnings("deprecation")
  protected void finalize() {
    delete();
  }

  public synchronized void delete() {
    if (swigCPtr != 0) {
      if (swigCMemOwn) {
        swigCMemOwn = false;
        AdaptiveCardObjectModelJNI.delete_ToggleVisibilityTargetVector(swigCPtr);
      }
      swigCPtr = 0;
    }
  }

  public ToggleVisibilityTargetVector(ToggleVisibilityTarget[] initialElements) {
    this();
    reserve(initialElements.length);

    for (ToggleVisibilityTarget element : initialElements) {
      add(element);
    }
  }

  public ToggleVisibilityTargetVector(Iterable<ToggleVisibilityTarget> initialElements) {
    this();
    for (ToggleVisibilityTarget element : initialElements) {
      add(element);
    }
  }

  public ToggleVisibilityTarget get(int index) {
    return doGet(index);
  }

  public ToggleVisibilityTarget set(int index, ToggleVisibilityTarget e) {
    return doSet(index, e);
  }

  public boolean add(ToggleVisibilityTarget e) {
    modCount++;
    doAdd(e);
    return true;
  }

  public void add(int index, ToggleVisibilityTarget e) {
    modCount++;
    doAdd(index, e);
  }

  public ToggleVisibilityTarget remove(int index) {
    modCount++;
    return doRemove(index);
  }

  protected void removeRange(int fromIndex, int toIndex) {
    modCount++;
    doRemoveRange(fromIndex, toIndex);
  }

  public int size() {
    return doSize();
  }

  public ToggleVisibilityTargetVector() {
    this(AdaptiveCardObjectModelJNI.new_ToggleVisibilityTargetVector__SWIG_0(), true);
  }

  public ToggleVisibilityTargetVector(ToggleVisibilityTargetVector other) {
    this(AdaptiveCardObjectModelJNI.new_ToggleVisibilityTargetVector__SWIG_1(ToggleVisibilityTargetVector.getCPtr(other), other), true);
  }

  public long capacity() {
    return AdaptiveCardObjectModelJNI.ToggleVisibilityTargetVector_capacity(swigCPtr, this);
  }

  public void reserve(long n) {
    AdaptiveCardObjectModelJNI.ToggleVisibilityTargetVector_reserve(swigCPtr, this, n);
  }

  public boolean isEmpty() {
    return AdaptiveCardObjectModelJNI.ToggleVisibilityTargetVector_isEmpty(swigCPtr, this);
  }

  public void clear() {
    AdaptiveCardObjectModelJNI.ToggleVisibilityTargetVector_clear(swigCPtr, this);
  }

  public ToggleVisibilityTargetVector(int count, ToggleVisibilityTarget value) {
    this(AdaptiveCardObjectModelJNI.new_ToggleVisibilityTargetVector__SWIG_2(count, ToggleVisibilityTarget.getCPtr(value), value), true);
  }

  private int doSize() {
    return AdaptiveCardObjectModelJNI.ToggleVisibilityTargetVector_doSize(swigCPtr, this);
  }

  private void doAdd(ToggleVisibilityTarget x) {
    AdaptiveCardObjectModelJNI.ToggleVisibilityTargetVector_doAdd__SWIG_0(swigCPtr, this, ToggleVisibilityTarget.getCPtr(x), x);
  }

  private void doAdd(int index, ToggleVisibilityTarget x) {
    AdaptiveCardObjectModelJNI.ToggleVisibilityTargetVector_doAdd__SWIG_1(swigCPtr, this, index, ToggleVisibilityTarget.getCPtr(x), x);
  }

  private ToggleVisibilityTarget doRemove(int index) {
    long cPtr = AdaptiveCardObjectModelJNI.ToggleVisibilityTargetVector_doRemove(swigCPtr, this, index);
    return (cPtr == 0) ? null : new ToggleVisibilityTarget(cPtr, true);
  }

  private ToggleVisibilityTarget doGet(int index) {
    long cPtr = AdaptiveCardObjectModelJNI.ToggleVisibilityTargetVector_doGet(swigCPtr, this, index);
    return (cPtr == 0) ? null : new ToggleVisibilityTarget(cPtr, true);
  }

  private ToggleVisibilityTarget doSet(int index, ToggleVisibilityTarget val) {
    long cPtr = AdaptiveCardObjectModelJNI.ToggleVisibilityTargetVector_doSet(swigCPtr, this, index, ToggleVisibilityTarget.getCPtr(val), val);
    return (cPtr == 0) ? null : new ToggleVisibilityTarget(cPtr, true);
  }

  private void doRemoveRange(int fromIndex, int toIndex) {
    AdaptiveCardObjectModelJNI.ToggleVisibilityTargetVector_doRemoveRange(swigCPtr, this, fromIndex, toIndex);
  }

}
