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

public class MemorySettings : global::System.IDisposable {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal MemorySettings(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  public static global::System.Runtime.InteropServices.HandleRef getCPtr(MemorySettings obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~MemorySettings() {
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
          Audio360CSharpPINVOKE.delete_MemorySettings(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
    }
  }

  public int spatDecoderQueuePoolSize {
    set {
      Audio360CSharpPINVOKE.MemorySettings_spatDecoderQueuePoolSize_set(swigCPtr, value);
    } 
    get {
      int ret = Audio360CSharpPINVOKE.MemorySettings_spatDecoderQueuePoolSize_get(swigCPtr);
      return ret;
    } 
  }

  public int spatDecoderFilePoolSize {
    set {
      Audio360CSharpPINVOKE.MemorySettings_spatDecoderFilePoolSize_set(swigCPtr, value);
    } 
    get {
      int ret = Audio360CSharpPINVOKE.MemorySettings_spatDecoderFilePoolSize_get(swigCPtr);
      return ret;
    } 
  }

  public int spatQueueSizePerChannel {
    set {
      Audio360CSharpPINVOKE.MemorySettings_spatQueueSizePerChannel_set(swigCPtr, value);
    } 
    get {
      int ret = Audio360CSharpPINVOKE.MemorySettings_spatQueueSizePerChannel_get(swigCPtr);
      return ret;
    } 
  }

  public int audioObjectPoolSize {
    set {
      Audio360CSharpPINVOKE.MemorySettings_audioObjectPoolSize_set(swigCPtr, value);
    } 
    get {
      int ret = Audio360CSharpPINVOKE.MemorySettings_audioObjectPoolSize_get(swigCPtr);
      return ret;
    } 
  }

  public uint speakersVirtualizersPoolSize {
    set {
      Audio360CSharpPINVOKE.MemorySettings_speakersVirtualizersPoolSize_set(swigCPtr, value);
    } 
    get {
      uint ret = Audio360CSharpPINVOKE.MemorySettings_speakersVirtualizersPoolSize_get(swigCPtr);
      return ret;
    } 
  }

  public AudioAssetManager audioAssetManager {
    set {
      Audio360CSharpPINVOKE.MemorySettings_audioAssetManager_set(swigCPtr, AudioAssetManager.getCPtr(value));
    } 
    get {
      global::System.IntPtr cPtr = Audio360CSharpPINVOKE.MemorySettings_audioAssetManager_get(swigCPtr);
      AudioAssetManager ret = (cPtr == global::System.IntPtr.Zero) ? null : new AudioAssetManager(cPtr, false);
      return ret;
    } 
  }

  public MemorySettings() : this(Audio360CSharpPINVOKE.new_MemorySettings(), true) {
  }

}

}
