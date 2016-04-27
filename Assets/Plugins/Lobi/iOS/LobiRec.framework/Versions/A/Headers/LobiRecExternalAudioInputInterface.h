#ifndef _Included_LobiRecExternalAudioInputInterface
#define _Included_LobiRecExternalAudioInputInterface
#ifdef __cplusplus
extern "C" {
#endif
    
    typedef void(*LobiRec_output_callback)(int16_t *buffer, int buffer_length, int samplerate, int channel_count);
    
    void LobiRec_set_output_callback(LobiRec_output_callback callback);
    
#ifdef __cplusplus
}
#endif
#endif