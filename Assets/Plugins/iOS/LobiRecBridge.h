#import <UIKit/UIKit.h>

#ifdef __cplusplus
extern "C" {
#endif
    #if UNITY_VERSION < 500
    void LobiRec_set_unity_pause_func(void(*unityPause)(bool)) __attribute__ ((deprecated));
    #else
    void LobiRec_set_unity_pause_func(void(*unityPause)(int)) __attribute__ ((deprecated));
    #endif
    void LobiRec_set_unity_gl_view(UIView*(*unityGetGLView)(void));
    UIView* LobiRec_get_unity_gl_view();    
#ifdef __cplusplus
}
#endif
