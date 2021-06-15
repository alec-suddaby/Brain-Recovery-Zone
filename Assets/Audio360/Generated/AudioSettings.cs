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

public class AudioSettings : global::System.IDisposable {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal AudioSettings(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  public static global::System.Runtime.InteropServices.HandleRef getCPtr(AudioSettings obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~AudioSettings() {
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
          Audio360CSharpPINVOKE.delete_AudioSettings(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
    }
  }

  public float sampleRate {
    set {
      Audio360CSharpPINVOKE.AudioSettings_sampleRate_set(swigCPtr, value);
    } 
    get {
      float ret = Audio360CSharpPINVOKE.AudioSettings_sampleRate_get(swigCPtr);
      return ret;
    } 
  }

  public int bufferSize {
    set {
      Audio360CSharpPINVOKE.AudioSettings_bufferSize_set(swigCPtr, value);
    } 
    get {
      int ret = Audio360CSharpPINVOKE.AudioSettings_bufferSize_get(swigCPtr);
      return ret;
    } 
  }

  public AudioDeviceType deviceType {
    set {
      Audio360CSharpPINVOKE.AudioSettings_deviceType_set(swigCPtr, (int)value);
    } 
    get {
      AudioDeviceType ret = (AudioDeviceType)Audio360CSharpPINVOKE.AudioSettings_deviceType_get(swigCPtr);
      return ret;
    } 
  }

  public string customAudioDeviceName {
    set {
      Audio360CSharpPINVOKE.AudioSettings_customAudioDeviceName_set(swigCPtr, value);
    } 
    get {
      string ret = Audio360CSharpPINVOKE.AudioSettings_customAudioDeviceName_get(swigCPtr);
      return ret;
    } 
  }

  public AudioSettings() : this(Audio360CSharpPINVOKE.new_AudioSettings(), true) {
  }

}

}
