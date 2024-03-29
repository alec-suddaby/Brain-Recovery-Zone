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

public class AudioFormatDecoder : global::System.IDisposable {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal AudioFormatDecoder(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  public static global::System.Runtime.InteropServices.HandleRef getCPtr(AudioFormatDecoder obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~AudioFormatDecoder() {
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
          Audio360CSharpPINVOKE.delete_AudioFormatDecoder(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
    }
  }

  public virtual int getNumOfChannels() {
    int ret = Audio360CSharpPINVOKE.AudioFormatDecoder_getNumOfChannels(swigCPtr);
    return ret;
  }

  public virtual uint getNumTotalSamples() {
    uint ret = Audio360CSharpPINVOKE.AudioFormatDecoder_getNumTotalSamples(swigCPtr);
    return ret;
  }

  public virtual uint getNumSamplesPerChannel() {
    uint ret = Audio360CSharpPINVOKE.AudioFormatDecoder_getNumSamplesPerChannel(swigCPtr);
    return ret;
  }

  public virtual double getMsPerChannel() {
    double ret = Audio360CSharpPINVOKE.AudioFormatDecoder_getMsPerChannel(swigCPtr);
    return ret;
  }

  public virtual uint getSamplePosition() {
    uint ret = Audio360CSharpPINVOKE.AudioFormatDecoder_getSamplePosition(swigCPtr);
    return ret;
  }

  public virtual EngineError seekToSample(uint samplePosition) {
    EngineError ret = (EngineError)Audio360CSharpPINVOKE.AudioFormatDecoder_seekToSample(swigCPtr, samplePosition);
    return ret;
  }

  public virtual uint decode(float[] bufferOut, int numOfSamplesInBuffer) {
    uint ret = Audio360CSharpPINVOKE.AudioFormatDecoder_decode(swigCPtr, bufferOut, numOfSamplesInBuffer);
    return ret;
  }

  public virtual float getSampleRate() {
    float ret = Audio360CSharpPINVOKE.AudioFormatDecoder_getSampleRate(swigCPtr);
    return ret;
  }

  public virtual float getOutputSampleRate() {
    float ret = Audio360CSharpPINVOKE.AudioFormatDecoder_getOutputSampleRate(swigCPtr);
    return ret;
  }

  public virtual int getNumBits() {
    int ret = Audio360CSharpPINVOKE.AudioFormatDecoder_getNumBits(swigCPtr);
    return ret;
  }

  public virtual bool endOfStream() {
    bool ret = Audio360CSharpPINVOKE.AudioFormatDecoder_endOfStream(swigCPtr);
    return ret;
  }

  public virtual bool decoderError() {
    bool ret = Audio360CSharpPINVOKE.AudioFormatDecoder_decoderError(swigCPtr);
    return ret;
  }

  public virtual int getMaxBufferSizePerChannel() {
    int ret = Audio360CSharpPINVOKE.AudioFormatDecoder_getMaxBufferSizePerChannel(swigCPtr);
    return ret;
  }

  public virtual string getName() {
    string ret = Audio360CSharpPINVOKE.AudioFormatDecoder_getName(swigCPtr);
    return ret;
  }

  public virtual void flush(bool resetToZero) {
    Audio360CSharpPINVOKE.AudioFormatDecoder_flush__SWIG_0(swigCPtr, resetToZero);
  }

  public virtual void flush() {
    Audio360CSharpPINVOKE.AudioFormatDecoder_flush__SWIG_1(swigCPtr);
  }

  public virtual int getInfo(AudioFormatDecoder.Info info) {
    int ret = Audio360CSharpPINVOKE.AudioFormatDecoder_getInfo(swigCPtr, (int)info);
    return ret;
  }

  public virtual ChannelMap getChannelMap() {
    ChannelMap ret = (ChannelMap)Audio360CSharpPINVOKE.AudioFormatDecoder_getChannelMap(swigCPtr);
    return ret;
  }

  public static AudioFormatDecoder create(string file, int maxBufferSizePerChannel, float outputSampleRate) {
    global::System.IntPtr cPtr = Audio360CSharpPINVOKE.AudioFormatDecoder_create__SWIG_0(file, maxBufferSizePerChannel, outputSampleRate);
    AudioFormatDecoder ret = (cPtr == global::System.IntPtr.Zero) ? null : new AudioFormatDecoder(cPtr, true);
    return ret;
  }

  public static AudioFormatDecoder create(string file, int maxBufferSizePerChannel) {
    global::System.IntPtr cPtr = Audio360CSharpPINVOKE.AudioFormatDecoder_create__SWIG_1(file, maxBufferSizePerChannel);
    AudioFormatDecoder ret = (cPtr == global::System.IntPtr.Zero) ? null : new AudioFormatDecoder(cPtr, true);
    return ret;
  }

  public static AudioFormatDecoder create(IOStream stream, bool decoderOwnsStream, int maxBufferSizePerChannel, float outputSampleRate) {
    global::System.IntPtr cPtr = Audio360CSharpPINVOKE.AudioFormatDecoder_create__SWIG_2(IOStream.getCPtr(stream), decoderOwnsStream, maxBufferSizePerChannel, outputSampleRate);
    AudioFormatDecoder ret = (cPtr == global::System.IntPtr.Zero) ? null : new AudioFormatDecoder(cPtr, false);
    return ret;
  }

  public static AudioFormatDecoder create(IOStream stream, bool decoderOwnsStream, int maxBufferSizePerChannel) {
    global::System.IntPtr cPtr = Audio360CSharpPINVOKE.AudioFormatDecoder_create__SWIG_3(IOStream.getCPtr(stream), decoderOwnsStream, maxBufferSizePerChannel);
    AudioFormatDecoder ret = (cPtr == global::System.IntPtr.Zero) ? null : new AudioFormatDecoder(cPtr, false);
    return ret;
  }

  public enum Info {
    PRE_SKIP
  }

}

}
