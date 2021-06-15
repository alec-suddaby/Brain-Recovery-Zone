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

public class VoiceManager : global::System.IDisposable {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal VoiceManager(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  public static global::System.Runtime.InteropServices.HandleRef getCPtr(VoiceManager obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~VoiceManager() {
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
          Audio360CSharpPINVOKE.delete_VoiceManager(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
    }
  }

  public virtual uint getMaxPhysicalVoices() {
    uint ret = Audio360CSharpPINVOKE.VoiceManager_getMaxPhysicalVoices(swigCPtr);
    return ret;
  }

  public virtual uint getMaxVirtualVoices() {
    uint ret = Audio360CSharpPINVOKE.VoiceManager_getMaxVirtualVoices(swigCPtr);
    return ret;
  }

  public virtual uint getMaxTotalVoices() {
    uint ret = Audio360CSharpPINVOKE.VoiceManager_getMaxTotalVoices(swigCPtr);
    return ret;
  }

  public virtual uint getNumPhysicalVoices() {
    uint ret = Audio360CSharpPINVOKE.VoiceManager_getNumPhysicalVoices(swigCPtr);
    return ret;
  }

  public virtual uint getNumVirtualVoices() {
    uint ret = Audio360CSharpPINVOKE.VoiceManager_getNumVirtualVoices(swigCPtr);
    return ret;
  }

  public virtual uint getNumTotalVoices() {
    uint ret = Audio360CSharpPINVOKE.VoiceManager_getNumTotalVoices(swigCPtr);
    return ret;
  }

  public virtual EngineError openVoice(out ulong voiceHandle, AudioAssetHandle assetHandle) {
    EngineError ret = (EngineError)Audio360CSharpPINVOKE.VoiceManager_openVoice(swigCPtr, out voiceHandle, AudioAssetHandle.getCPtr(assetHandle));
    if (Audio360CSharpPINVOKE.SWIGPendingException.Pending) throw Audio360CSharpPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public virtual EngineError closeVoice(ulong voiceHandle) {
    EngineError ret = (EngineError)Audio360CSharpPINVOKE.VoiceManager_closeVoice(swigCPtr, voiceHandle);
    return ret;
  }

  public virtual bool voiceIsOpen(ulong voiceHandle) {
    bool ret = Audio360CSharpPINVOKE.VoiceManager_voiceIsOpen(swigCPtr, voiceHandle);
    return ret;
  }

  public virtual EngineError play(ulong voiceHandle, float delayMs, float fadeTimeMs) {
    EngineError ret = (EngineError)Audio360CSharpPINVOKE.VoiceManager_play(swigCPtr, voiceHandle, delayMs, fadeTimeMs);
    return ret;
  }

  public virtual EngineError pause(ulong voiceHandle, float delayMs, float fadeTimeMs) {
    EngineError ret = (EngineError)Audio360CSharpPINVOKE.VoiceManager_pause(swigCPtr, voiceHandle, delayMs, fadeTimeMs);
    return ret;
  }

  public virtual EngineError stop(ulong voiceHandle, float delayMs, float fadeTimeMs) {
    EngineError ret = (EngineError)Audio360CSharpPINVOKE.VoiceManager_stop(swigCPtr, voiceHandle, delayMs, fadeTimeMs);
    return ret;
  }

  public virtual EngineError getPlayState(ulong voiceHandle, out PlayState playState) {
    EngineError ret = (EngineError)Audio360CSharpPINVOKE.VoiceManager_getPlayState(swigCPtr, voiceHandle, out playState);
    return ret;
  }

  public virtual EngineError seekMs(ulong voiceHandle, float posMs) {
    EngineError ret = (EngineError)Audio360CSharpPINVOKE.VoiceManager_seekMs(swigCPtr, voiceHandle, posMs);
    return ret;
  }

  public virtual EngineError getElapsedTimeMs(ulong voiceHandle, ref float timeMs) {
    EngineError ret = (EngineError)Audio360CSharpPINVOKE.VoiceManager_getElapsedTimeMs(swigCPtr, voiceHandle, ref timeMs);
    return ret;
  }

  public virtual EngineError getDurationMs(ulong voiceHandle, ref float timeMs) {
    EngineError ret = (EngineError)Audio360CSharpPINVOKE.VoiceManager_getDurationMs(swigCPtr, voiceHandle, ref timeMs);
    return ret;
  }

  public virtual EngineError setParam(ulong voiceHandle, VoiceParam param, float value) {
    EngineError ret = (EngineError)Audio360CSharpPINVOKE.VoiceManager_setParam(swigCPtr, voiceHandle, (int)param, value);
    return ret;
  }

  public virtual EngineError getParam(ulong voiceHandle, VoiceParam param, ref float value) {
    EngineError ret = (EngineError)Audio360CSharpPINVOKE.VoiceManager_getParam(swigCPtr, voiceHandle, (int)param, ref value);
    return ret;
  }

  public virtual EngineError getParamDescription(VoiceParam param, VoiceParamDescription description) {
    EngineError ret = (EngineError)Audio360CSharpPINVOKE.VoiceManager_getParamDescription(swigCPtr, (int)param, VoiceParamDescription.getCPtr(description));
    if (Audio360CSharpPINVOKE.SWIGPendingException.Pending) throw Audio360CSharpPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public virtual EngineError setBus(ulong voiceHandle, global::System.IntPtr bus) {
    EngineError ret = (EngineError)Audio360CSharpPINVOKE.VoiceManager_setBus(swigCPtr, voiceHandle, bus);
    return ret;
  }

  public virtual EngineError getBus(ulong voiceHandle, out global::System.IntPtr bus) {
    EngineError ret = (EngineError)Audio360CSharpPINVOKE.VoiceManager_getBus(swigCPtr, voiceHandle, out bus);
    return ret;
  }

  public virtual EngineError getVoiceMode(ulong voiceHandle, out VoiceMode mode) {
    EngineError ret = (EngineError)Audio360CSharpPINVOKE.VoiceManager_getVoiceMode(swigCPtr, voiceHandle, out mode);
    return ret;
  }

  public virtual EngineError setEventCallback(VoiceManagerEventCb callback, global::System.IntPtr userData) {
    EngineError ret = (EngineError)Audio360CSharpPINVOKE.VoiceManager_setEventCallback(swigCPtr, callback, userData);
    return ret;
  }

}

}