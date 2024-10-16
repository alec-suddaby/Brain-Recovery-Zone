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

public class DirectionalProps : global::System.IDisposable {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal DirectionalProps(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  public static global::System.Runtime.InteropServices.HandleRef getCPtr(DirectionalProps obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~DirectionalProps() {
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
          Audio360CSharpPINVOKE.delete_DirectionalProps(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
    }
  }

  public float effectLevel {
    set {
      Audio360CSharpPINVOKE.DirectionalProps_effectLevel_set(swigCPtr, value);
    } 
    get {
      float ret = Audio360CSharpPINVOKE.DirectionalProps_effectLevel_get(swigCPtr);
      return ret;
    } 
  }

  public float coneArea {
    set {
      Audio360CSharpPINVOKE.DirectionalProps_coneArea_set(swigCPtr, value);
    } 
    get {
      float ret = Audio360CSharpPINVOKE.DirectionalProps_coneArea_get(swigCPtr);
      return ret;
    } 
  }

  public DirectionalProps() : this(Audio360CSharpPINVOKE.new_DirectionalProps__SWIG_0(), true) {
  }

  public DirectionalProps(float effectLevelValue, float coneAreaValue) : this(Audio360CSharpPINVOKE.new_DirectionalProps__SWIG_1(effectLevelValue, coneAreaValue), true) {
  }

  public void set(float effectLevelValue, float coneAreaValue) {
    Audio360CSharpPINVOKE.DirectionalProps_set(swigCPtr, effectLevelValue, coneAreaValue);
  }

}

}
