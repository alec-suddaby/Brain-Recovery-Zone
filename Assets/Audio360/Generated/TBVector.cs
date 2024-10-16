// Copyright (c) 2018-present, Facebook, Inc. 
// @generated
//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 4.0.1
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------

namespace TBE {

public class TBVector : global::System.IDisposable {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal TBVector(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  public static global::System.Runtime.InteropServices.HandleRef getCPtr(TBVector obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~TBVector() {
    Dispose(false);
  }

  public void Dispose() {
    Dispose(true);
    global::System.GC.SuppressFinalize(this);
  }

  protected virtual void Dispose(bool disposing) {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          Audio360CSharpPINVOKE.delete_TBVector(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
    }
  }

  public float x {
    set {
      Audio360CSharpPINVOKE.TBVector_x_set(swigCPtr, value);
    } 
    get {
      float ret = Audio360CSharpPINVOKE.TBVector_x_get(swigCPtr);
      return ret;
    } 
  }

  public float y {
    set {
      Audio360CSharpPINVOKE.TBVector_y_set(swigCPtr, value);
    } 
    get {
      float ret = Audio360CSharpPINVOKE.TBVector_y_get(swigCPtr);
      return ret;
    } 
  }

  public float z {
    set {
      Audio360CSharpPINVOKE.TBVector_z_set(swigCPtr, value);
    } 
    get {
      float ret = Audio360CSharpPINVOKE.TBVector_z_get(swigCPtr);
      return ret;
    } 
  }

  public TBVector() : this(Audio360CSharpPINVOKE.new_TBVector__SWIG_0(), true) {
  }

  public TBVector(float xValue, float yValue, float zValue) : this(Audio360CSharpPINVOKE.new_TBVector__SWIG_1(xValue, yValue, zValue), true) {
  }

  public TBVector(float value) : this(Audio360CSharpPINVOKE.new_TBVector__SWIG_2(value), true) {
  }

  public static TBVector CrossProduct(TBVector vectorA, TBVector vectorB) {
    TBVector ret = new TBVector(Audio360CSharpPINVOKE.TBVector_CrossProduct(TBVector.getCPtr(vectorA), TBVector.getCPtr(vectorB)), true);
    if (Audio360CSharpPINVOKE.SWIGPendingException.Pending) throw Audio360CSharpPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public static TBVector zero() {
    TBVector ret = new TBVector(Audio360CSharpPINVOKE.TBVector_zero(), true);
    return ret;
  }

  public static TBVector forward() {
    TBVector ret = new TBVector(Audio360CSharpPINVOKE.TBVector_forward(), true);
    return ret;
  }

  public static TBVector up() {
    TBVector ret = new TBVector(Audio360CSharpPINVOKE.TBVector_up(), true);
    return ret;
  }

  public static float DotProduct(TBVector vectorA, TBVector vectorB) {
    float ret = Audio360CSharpPINVOKE.TBVector_DotProduct(TBVector.getCPtr(vectorA), TBVector.getCPtr(vectorB));
    if (Audio360CSharpPINVOKE.SWIGPendingException.Pending) throw Audio360CSharpPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public static float Angle(TBVector vectorA, TBVector vectorB) {
    float ret = Audio360CSharpPINVOKE.TBVector_Angle(TBVector.getCPtr(vectorA), TBVector.getCPtr(vectorB));
    if (Audio360CSharpPINVOKE.SWIGPendingException.Pending) throw Audio360CSharpPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public static float magnitude(TBVector vector) {
    float ret = Audio360CSharpPINVOKE.TBVector_magnitude(TBVector.getCPtr(vector));
    if (Audio360CSharpPINVOKE.SWIGPendingException.Pending) throw Audio360CSharpPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public static void clampMagnitude(TBVector vector, float maxValue) {
    Audio360CSharpPINVOKE.TBVector_clampMagnitude(TBVector.getCPtr(vector), maxValue);
    if (Audio360CSharpPINVOKE.SWIGPendingException.Pending) throw Audio360CSharpPINVOKE.SWIGPendingException.Retrieve();
  }

  public static float magSquared(TBVector vector) {
    float ret = Audio360CSharpPINVOKE.TBVector_magSquared(TBVector.getCPtr(vector));
    if (Audio360CSharpPINVOKE.SWIGPendingException.Pending) throw Audio360CSharpPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public static void normalise(TBVector vector) {
    Audio360CSharpPINVOKE.TBVector_normalise(TBVector.getCPtr(vector));
    if (Audio360CSharpPINVOKE.SWIGPendingException.Pending) throw Audio360CSharpPINVOKE.SWIGPendingException.Retrieve();
  }

  public void set(float xValue, float yValue, float zValue) {
    Audio360CSharpPINVOKE.TBVector_set(swigCPtr, xValue, yValue, zValue);
  }

  public void abs() {
    Audio360CSharpPINVOKE.TBVector_abs__SWIG_0(swigCPtr);
  }

  public static TBVector projectOntoPlane(TBVector inputVector, TBVector unitNormalToPlane) {
    TBVector ret = new TBVector(Audio360CSharpPINVOKE.TBVector_projectOntoPlane(TBVector.getCPtr(inputVector), TBVector.getCPtr(unitNormalToPlane)), true);
    if (Audio360CSharpPINVOKE.SWIGPendingException.Pending) throw Audio360CSharpPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public static void rotateByVectors(TBVector forwardVector, TBVector upVector, TBVector rotatedVectorOut) {
    Audio360CSharpPINVOKE.TBVector_rotateByVectors(TBVector.getCPtr(forwardVector), TBVector.getCPtr(upVector), TBVector.getCPtr(rotatedVectorOut));
    if (Audio360CSharpPINVOKE.SWIGPendingException.Pending) throw Audio360CSharpPINVOKE.SWIGPendingException.Retrieve();
  }

  public float min_val() {
    float ret = Audio360CSharpPINVOKE.TBVector_min_val(swigCPtr);
    return ret;
  }

  public float max_val() {
    float ret = Audio360CSharpPINVOKE.TBVector_max_val(swigCPtr);
    return ret;
  }

  public string toString(int precision) {
    string ret = Audio360CSharpPINVOKE.TBVector_toString__SWIG_0(swigCPtr, precision);
    return ret;
  }

  public string toString() {
    string ret = Audio360CSharpPINVOKE.TBVector_toString__SWIG_1(swigCPtr);
    return ret;
  }

  public static TBVector getVectorFromAziEle(float azimuth, float elevation) {
    TBVector ret = new TBVector(Audio360CSharpPINVOKE.TBVector_getVectorFromAziEle(azimuth, elevation), true);
    return ret;
  }

  public static TBVector getVectorFromAziEleDist(float azimuth, float elevation, float dist) {
    TBVector ret = new TBVector(Audio360CSharpPINVOKE.TBVector_getVectorFromAziEleDist(azimuth, elevation, dist), true);
    return ret;
  }

  public static TBVector getVectorFromEuler(TBVector eulerAngles) {
    TBVector ret = new TBVector(Audio360CSharpPINVOKE.TBVector_getVectorFromEuler(TBVector.getCPtr(eulerAngles)), true);
    if (Audio360CSharpPINVOKE.SWIGPendingException.Pending) throw Audio360CSharpPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

}

}
