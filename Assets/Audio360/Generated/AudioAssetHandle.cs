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

public class AudioAssetHandle : global::System.IDisposable {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal AudioAssetHandle(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  public static global::System.Runtime.InteropServices.HandleRef getCPtr(AudioAssetHandle obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~AudioAssetHandle() {
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
          Audio360CSharpPINVOKE.delete_AudioAssetHandle(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
    }
  }

  public uint index {
    set {
      Audio360CSharpPINVOKE.AudioAssetHandle_index_set(swigCPtr, value);
    } 
    get {
      uint ret = Audio360CSharpPINVOKE.AudioAssetHandle_index_get(swigCPtr);
      return ret;
    } 
  }

  public uint id {
    set {
      Audio360CSharpPINVOKE.AudioAssetHandle_id_set(swigCPtr, value);
    } 
    get {
      uint ret = Audio360CSharpPINVOKE.AudioAssetHandle_id_get(swigCPtr);
      return ret;
    } 
  }

  public AudioAssetHandle() : this(Audio360CSharpPINVOKE.new_AudioAssetHandle(), true) {
  }

}

}
