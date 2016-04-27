#import <UIKit/UIKit.h>

#import <LobiCore/LobiCore.h>
#import <LobiRec/LobiRec.h>
#import <LobiRec/LobiRec+Metal.h>

#import "LobiCoreBridge.h"
#import "LobiCoreCommon.h"
#import "LobiRecBridge.h"

#ifdef DEBUG
#define KLV_IMAGE_PATH @"%@/Documents/p/"
#else
#define KLV_IMAGE_PATH @"%@/lobiImageContents/"
#endif

@interface LobiRecUnityObserver : NSObject
@property (nonatomic, copy) NSString *gameObjectName;
@property (nonatomic, copy) NSString *callbackMethodName;
@property (nonatomic, copy) NSString *name;
- (void)register:(NSString*)gameObjectName callbackMethod:(NSString*)callbackMethodName;
- (void)unregister;
- (void)notify:(NSNotification*)notification;
@end

@implementation LobiRecUnityObserver

- (void)dealloc{
    self.gameObjectName = nil;
    self.callbackMethodName = nil;
    self.name = nil;
    [super dealloc];
}

- (void)register:(NSString*)gameObjectName callbackMethod:(NSString*)callbackMethodName
{
    [self unregister];
    self.gameObjectName = gameObjectName;
    self.callbackMethodName = callbackMethodName;
    [[NSNotificationCenter defaultCenter] addObserver:self
                                             selector:@selector(notify:)
                                                 name:self.name
                                               object:nil];
}

- (void)unregister
{
    self.gameObjectName = nil;
    self.callbackMethodName = nil;
    [[NSNotificationCenter defaultCenter] removeObserver:self];
}

- (void)notify:(NSNotification*)notification
{
    NSString *gameObjectName = [[self.gameObjectName copy] autorelease];
    NSString *callbackMethodName = [[self.callbackMethodName copy] autorelease];
    
    if(gameObjectName != nil && [gameObjectName length] != 0 && callbackMethodName != nil && [callbackMethodName length] != 0){
        UnitySendMessage([gameObjectName cStringUsingEncoding:NSUTF8StringEncoding],
                         [callbackMethodName cStringUsingEncoding:NSUTF8StringEncoding],
                         [@"" cStringUsingEncoding:NSUTF8StringEncoding]);
    }
}

@end

#pragma mark -


@interface LobiRecMicEnableErrorObserver : LobiRecUnityObserver
+ (instancetype) sharedInstance;
@end

@implementation LobiRecMicEnableErrorObserver
- (instancetype)init
{
    self = [super init];
    if(self != nil){
        self.name = KLVMicEnableErrorNotification;
    }
    return self;
}

+ (instancetype) sharedInstance
{
    static id sharedInstance = nil;
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        sharedInstance = [[self.class alloc] init];
    });
    return sharedInstance;
}
@end

void LobiRec_mic_enable_error_observer_(const char *game_object_name, int game_object_name_len,
                                        const char *callback_method_name, int callback_method_name_len)
{
    NSString *gameObjectName     = make_autorelease_string(game_object_name);
    NSString *callbackMethodName = make_autorelease_string(callback_method_name);
    [[LobiRecMicEnableErrorObserver sharedInstance] register:gameObjectName callbackMethod:callbackMethodName];
}

void LobiRec_unregister_mic_enable_error_observer_()
{
    [[LobiRecMicEnableErrorObserver sharedInstance] unregister];
}

// 残り容量チェック
@interface LobiRecDryingUpInStorageObserver : LobiRecUnityObserver
+ (instancetype) sharedInstance;
@end

@implementation LobiRecDryingUpInStorageObserver
- (instancetype)init
{
    self = [super init];
    if(self != nil){
        self.name = KLVDryingUpInStorageNotification;
    }
    return self;
}

+ (instancetype) sharedInstance
{
    static id sharedInstance = nil;
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        sharedInstance = [[self.class alloc] init];
    });
    return sharedInstance;
}

@end

void LobiRec_register_drying_up_in_storage_observer_(const char *game_object_name, int game_object_name_len,
                                                     const char *callback_method_name, int callback_method_name_len)
{
    NSString *gameObjectName     = make_autorelease_string(game_object_name);
    NSString *callbackMethodName = make_autorelease_string(callback_method_name);
    [[LobiRecDryingUpInStorageObserver sharedInstance] register:gameObjectName callbackMethod:callbackMethodName];
}

void LobiRec_unregister_drying_up_in_storage_observer_()
{
    [[LobiRecDryingUpInStorageObserver sharedInstance] unregister];
}

#pragma mark -
// 投稿開始
@interface LobiRecMovieCreatedNotification : LobiRecUnityObserver
+ (instancetype) sharedInstance;
@end

@implementation LobiRecMovieCreatedNotification

- (instancetype)init
{
    self = [super init];
    if(self != nil){
        self.name = KLVMovieCreatedNotification;
    }
    return self;
}

+ (instancetype) sharedInstance
{
    static id sharedInstance = nil;
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        sharedInstance = [[self.class alloc] init];
    });
    return sharedInstance;
}

- (void)notify:(NSNotification*)notification
{
    NSString *gameObjectName = [[self.gameObjectName copy] autorelease];
    NSString *callbackMethodName = [[self.callbackMethodName copy] autorelease];
    
    if(gameObjectName != nil && [gameObjectName length] != 0 && callbackMethodName != nil && [callbackMethodName length] != 0){
        NSString *url = notification.userInfo[@"url"];
        url = (url == nil ? @"" : url);
        NSString *videoId = notification.userInfo[@"videoId"];
        videoId = (videoId == nil ? @"" : videoId);
        NSString *jsonStr = [NSString stringWithFormat:@"{\"url\": \"%@\", \"videoId\": \"%@\"}", url, videoId];
        
        UnitySendMessage([gameObjectName cStringUsingEncoding:NSUTF8StringEncoding],
                         [callbackMethodName cStringUsingEncoding:NSUTF8StringEncoding],
                         [jsonStr cStringUsingEncoding:NSUTF8StringEncoding]);
    }
}

@end

void LobiRec_register_movie_created_notification_(const char *game_object_name, int game_object_name_len,
                                                  const char *callback_method_name, int callback_method_name_len)
{
    NSString *gameObjectName     = make_autorelease_string(game_object_name);
    NSString *callbackMethodName = make_autorelease_string(callback_method_name);
    [[LobiRecMovieCreatedNotification sharedInstance] register:gameObjectName callbackMethod:callbackMethodName];
}

void LobiRec_unregister_movie_created_notification_()
{
    [[LobiRecMovieCreatedNotification sharedInstance] unregister];
}

#pragma mark -
// 投稿開始失敗
@interface LobiRecMovieCreatedErrorNotification : LobiRecUnityObserver
+ (instancetype) sharedInstance;
@end

@implementation LobiRecMovieCreatedErrorNotification

- (instancetype)init
{
    self = [super init];
    if(self != nil){
        self.name = KLVMovieCreatedErrorNotification;
    }
    return self;
}

+ (instancetype) sharedInstance
{
    static id sharedInstance = nil;
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        sharedInstance = [[self.class alloc] init];
    });
    return sharedInstance;
}

@end

void LobiRec_register_movie_created_error_notification_(const char *game_object_name, int game_object_name_len,
                                                        const char *callback_method_name, int callback_method_name_len)
{
    NSString *gameObjectName     = make_autorelease_string(game_object_name);
    NSString *callbackMethodName = make_autorelease_string(callback_method_name);
    [[LobiRecMovieCreatedErrorNotification sharedInstance] register:gameObjectName callbackMethod:callbackMethodName];
}

void LobiRec_unregister_movie_created_error_notification_()
{
    [[LobiRecMovieCreatedErrorNotification sharedInstance] unregister];
}

#pragma mark -
// 投稿完了
@interface LobiRecMovieUploadedNotification : LobiRecUnityObserver
+ (instancetype) sharedInstance;
@end

@implementation LobiRecMovieUploadedNotification

- (instancetype)init
{
    self = [super init];
    if(self != nil){
        self.name = KLVMovieUploadedNotification;
    }
    return self;
}

+ (instancetype) sharedInstance
{
    static id sharedInstance = nil;
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        sharedInstance = [[self.class alloc] init];
    });
    return sharedInstance;
}

@end

void LobiRec_register_movie_uploaded_notification_(const char *game_object_name, int game_object_name_len,
                                                   const char *callback_method_name, int callback_method_name_len)
{
    NSString *gameObjectName     = make_autorelease_string(game_object_name);
    NSString *callbackMethodName = make_autorelease_string(callback_method_name);
    [[LobiRecMovieUploadedNotification sharedInstance] register:gameObjectName callbackMethod:callbackMethodName];
}

void LobiRec_unregister_movie_uploaded_notification_()
{
    [[LobiRecMovieUploadedNotification sharedInstance] unregister];
}

#pragma mark -
// 投稿中失敗
@interface LobiRecMovieUploadedErrorNotification : LobiRecUnityObserver
+ (instancetype) sharedInstance;
@end

@implementation LobiRecMovieUploadedErrorNotification

- (instancetype)init
{
    self = [super init];
    if(self != nil){
        self.name = KLVMovieUploadedErrorNotification;
    }
    return self;
}

+ (instancetype) sharedInstance
{
    static id sharedInstance = nil;
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        sharedInstance = [[self.class alloc] init];
    });
    return sharedInstance;
}

@end

void LobiRec_register_movie_uploaded_error_notification_(const char *game_object_name, int game_object_name_len,
                                                         const char *callback_method_name, int callback_method_name_len)
{
    NSString *gameObjectName     = make_autorelease_string(game_object_name);
    NSString *callbackMethodName = make_autorelease_string(callback_method_name);
    [[LobiRecMovieUploadedErrorNotification sharedInstance] register:gameObjectName callbackMethod:callbackMethodName];
}

void LobiRec_unregister_movie_uploaded_error_notification_()
{
    [[LobiRecMovieUploadedErrorNotification sharedInstance] unregister];
}

// 投稿画面を閉じた
@interface LobiRecDismissingPostVideoViewControllerNotification : LobiRecUnityObserver
+ (instancetype) sharedInstance;
@end

@implementation LobiRecDismissingPostVideoViewControllerNotification

- (instancetype)init
{
    self = [super init];
    if(self != nil){
        self.name = KLVDismissingPostVideoViewControllerNotification;
    }
    return self;
}

+ (instancetype) sharedInstance
{
    static id sharedInstance = nil;
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        sharedInstance = [[self.class alloc] init];
    });
    return sharedInstance;
}

- (void)notify:(NSNotification*)notification
{
    NSString *gameObjectName = [[self.gameObjectName copy] autorelease];
    NSString *callbackMethodName = [[self.callbackMethodName copy] autorelease];
    
    if(gameObjectName != nil && [gameObjectName length] != 0 && callbackMethodName != nil && [callbackMethodName length] != 0){
        BOOL isTryed = [notification.userInfo[@"tryPost"] boolValue];
        BOOL isTwitterShare = [notification.userInfo[@"twitter_share"] boolValue];
        BOOL isFacebookShare = [notification.userInfo[@"facebook_share"] boolValue];
        BOOL isYoutubeShare = [notification.userInfo[@"youtube_share"] boolValue];
        BOOL isNicovideoShare = [notification.userInfo[@"nicovideo_share"] boolValue];
        NSString *jsonStr = [NSString stringWithFormat:@"{\"try_post\": %@, \"twitter_share\": %@, \"facebook_share\": %@, \"youtube_share\": %@, \"nicovideo_share\": %@}",
                             isTryed ? @"1" : @"0",
                             isTwitterShare ? @"1" : @"0",
                             isFacebookShare ? @"1" : @"0",
                             isYoutubeShare ? @"1" : @"0",
                             isNicovideoShare ? @"1" : @"0"];
        
        UnitySendMessage([gameObjectName cStringUsingEncoding:NSUTF8StringEncoding],
                         [callbackMethodName cStringUsingEncoding:NSUTF8StringEncoding],
                         [jsonStr cStringUsingEncoding:NSUTF8StringEncoding]);
    }
}

@end

void LobiRec_register_dismissing_post_video_view_controller_notification_(const char *game_object_name, int game_object_name_len,
                                                                          const char *callback_method_name, int callback_method_name_len)
{
    NSString *gameObjectName     = make_autorelease_string(game_object_name);
    NSString *callbackMethodName = make_autorelease_string(callback_method_name);
    [[LobiRecDismissingPostVideoViewControllerNotification sharedInstance] register:gameObjectName callbackMethod:callbackMethodName];
}

void LobiRec_unregister_dismissing_post_video_view_controller_notification_()
{
    [[LobiRecDismissingPostVideoViewControllerNotification sharedInstance] unregister];
}

# pragma mark -
static UIView*(*sUnityGetGLView)(void);

void LobiRec_set_unity_gl_view(UIView*(*getGLView)())
{
    sUnityGetGLView = getGLView;
}

UIView* LobiRec_get_unity_gl_view()
{
    return sUnityGetGLView();
}

#if UNITY_VERSION < 500
void LobiRec_set_unity_pause_func(void(*unityPause)(bool))
{
    LobiCore_set_unity_pause_func(unityPause);
}
#else
void LobiRec_set_unity_pause_func(void(*unityPause)(int))
{
    LobiCore_set_unity_pause_func(unityPause);
}
#endif

void LobiRec_set_sticky_recording_(int enable)
{
    [LobiRec sharedInstance].stickyRecording = (enable == 1);
}

int LobiRec_get_sticky_recording_()
{
    return [LobiRec sharedInstance].stickyRecording ? 1 : 0;
}

int LobiRec_is_capturing_()
{
    return [LobiRec sharedInstance].isCapturing ? 1 : 0;
}

void LobiRec_set_live_wipe_status_(int status)
{
    [LobiRec sharedInstance].liveWipeStatus = status;
}

int LobiRec_get_live_wipe_status_()
{
    return [LobiRec sharedInstance].liveWipeStatus;
}

void LobiRec_set_wipe_position_x_(float x)
{
    [LobiRec sharedInstance].wipePositionX = x;
}

float LobiRec_get_wipe_position_x_()
{
    return [LobiRec sharedInstance].wipePositionX;
}

void LobiRec_set_wipe_position_y_(float y)
{
    [LobiRec sharedInstance].wipePositionY = y;
}

float LobiRec_get_wipe_position_y_()
{
    return [LobiRec sharedInstance].wipePositionY;
}

void LobiRec_set_wipe_square_size_(float size)
{
    [LobiRec sharedInstance].wipeSquareSize = size;
}

float LobiRec_get_wipe_square_size_()
{
    return [LobiRec sharedInstance].wipeSquareSize;
}

void LobiRec_set_game_sound_volume_(float volume)
{
    [LobiRec sharedInstance].gameSoundVolume = volume;
}

float LobiRec_get_game_sound_volume_()
{
    return [LobiRec sharedInstance].gameSoundVolume;
}

void LobiRec_set_after_recording_volume_(float volume)
{
    [LobiRec sharedInstance].afterRecordingVolume = volume;
}

float LobiRec_get_after_recording_volume_()
{
    return [LobiRec sharedInstance].afterRecordingVolume;
}

void LobiRec_set_mic_volume_(float volume)
{
    [LobiRec sharedInstance].micVolume = volume;
}

float LobiRec_get_mic_volume_()
{
    return [LobiRec sharedInstance].micVolume;
}

void LobiRec_set_mic_enable_(int enable)
{
    [LobiRec sharedInstance].micEnable = (enable == 1);
}

int LobiRec_get_mic_enable_()
{
    return [LobiRec sharedInstance].micEnable ? 1 : 0;
}

void LobiRec_set_prevent_spoiler_(int enable)
{
    [LobiRec sharedInstance].preventSpoiler = (enable == 1);
}

int LobiRec_get_prevent_spoiler_()
{
    return [LobiRec sharedInstance].preventSpoiler ? 1 : 0;
}

void LobiRec_set_hide_face_on_preview_(int enable)
{
    [LobiRec sharedInstance].hideFaceOnPreview = (enable == 1);
}

int LobiRec_get_hide_face_on_preview_()
{
    return [LobiRec sharedInstance].hideFaceOnPreview ? 1 : 0;
}

void LobiRec_set_capture_per_frame_(int frame)
{
    [LobiRec sharedInstance].capturePerFrame = frame;
}

int LobiRec_get_capture_per_frame_()
{
    return [LobiRec sharedInstance].capturePerFrame;
}

void LobiRec_start_capturing_()
{
    [LobiRec startCapturing];
}

void LobiRec_stop_capturing_()
{
    [LobiRec stopCapturing];
}

void LobiRec_resume_capturing_()
{
    [LobiRec resume];
}

void LobiRec_pause_capturing_()
{
    [LobiRec pause];
}

int LobiRec_is_paused_()
{
    return [LobiRec isPaused] ? 1 : 0;
}

int LobiRec_has_movie_()
{
    return [LobiRec hasMovie] ? 1 : 0;
}

void LobiRec_present_lobi_play_()
{
    [LobiCore setRootViewController:LobiCore_get_root_view_controller()];
    [LobiRec presentLobiPlay:^
     {
         LobiCore_prepare_handler();
     }
                afterHandler:^
     {
         LobiCore_after_handler();
     }];
}

void LobiRec_present_lobi_play_with_video_id_(const char* video_id, int video_id_len)
{
    NSString *videoId = make_autorelease_string(video_id);
    
    [LobiCore setRootViewController:LobiCore_get_root_view_controller()];
    [LobiRec presentLobiPlay:videoId
              prepareHandler:^
     {
         LobiCore_prepare_handler();
     }
                afterHandler:^
     {
         LobiCore_after_handler();
     }];
}

void LobiRec_stop_capturing_with_callback_(const char *game_object_name, int  game_object_name_len,
                                           const char *callback_method_name, int callback_method_name_len)
{
    NSString *gameObjectName     = make_autorelease_string(game_object_name);
    NSString *callbackMethodName = make_autorelease_string(callback_method_name);
    [LobiRec stopCapturingWithHandler:^{
        UnitySendMessage([gameObjectName cStringUsingEncoding:NSUTF8StringEncoding],
                         [callbackMethodName cStringUsingEncoding:NSUTF8StringEncoding],
                         [@"" cStringUsingEncoding:NSUTF8StringEncoding]);
    }];
}

void LobiRec_present_lobi_play_with_conditions_(const char* play_user_exid, int play_user_exid_len,
                                                const char* play_category, int play_category_len,
                                                int play_lets_play,
                                                const char* play_meta_fields, int play_meta_fields_len)
{
    NSString *playUserExId   = make_autorelease_string(play_user_exid);
    NSString *playCategory   = make_autorelease_string(play_category);
    BOOL      playLetsPlay   = (play_lets_play == 1);
    NSString *playMetaFields = make_autorelease_string(play_meta_fields);
    
    [LobiCore setRootViewController:LobiCore_get_root_view_controller()];
    [LobiRec presentLobiPlay:playUserExId
                    category:playCategory
                    letsPlay:playLetsPlay
                  metaFields:playMetaFields
              prepareHandler:^
     {
         LobiCore_prepare_handler();
     }
                afterHandler:^
     {
         LobiCore_after_handler();
     }];
}

void LobiRec_present_lobi_play_with_event_fields_(const char* event_fields, int event_fields_len)
{
    NSString *eventFields = make_autorelease_string(event_fields);
    
    [LobiCore setRootViewController:LobiCore_get_root_view_controller()];
    [LobiRec presentLobiPlayWithEventFields:eventFields
                             prepareHandler:^
     {
         LobiCore_prepare_handler();
     }
                               afterHandler:^
    {
         LobiCore_after_handler();

     }];
}

void LobiRec_present_lobi_post_(const char* title, int title_len,
                                const char* post_description, int post_description_len,
                                int64_t post_score,
                                const char* post_category, int post_category_len,
                                const char* post_metadata, int post_metadata_len)
{
    NSString *t               = make_autorelease_string(title);
    NSString *postDescription = make_autorelease_string(post_description);
    NSString *postCategory    = make_autorelease_string(post_category);
    NSString *postMetadata    = make_autorelease_string(post_metadata);
    [LobiCore setRootViewController:LobiCore_get_root_view_controller()];
    [LobiRec presentLobiPostWithTitle:t
                       postDescrition:postDescription
                            postScore:post_score
                         postCategory:postCategory
                         postMetaData:postMetadata
                       prepareHandler:^
     {
         LobiCore_prepare_handler();
     }
                         afterHandler:^
     {
         LobiCore_after_handler();
     }];
}


void LobiRec_snap_(const char *game_object_name, int  game_object_name_len,
                   const char *callback_method_name, int callback_method_name_len)
{
    NSString *gameObjectName     = make_autorelease_string(game_object_name);
    NSString *callbackMethodName = make_autorelease_string(callback_method_name);
    [LobiRec snap:^(UIImage *image) {
#ifdef DEBUG
        NSString *path = [NSString stringWithFormat:KLV_IMAGE_PATH, NSHomeDirectory()];
#else
        NSString *path = [NSString stringWithFormat:KLV_IMAGE_PATH, NSTemporaryDirectory()];
#endif
        
        dispatch_async(dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_DEFAULT, 0), ^{
            
            NSDateFormatter* formatter = [[NSDateFormatter alloc] init];
            NSLocale* local = [[NSLocale alloc] initWithLocaleIdentifier:@"ja_JP"];
            [formatter setLocale:local];
            [formatter setTimeZone:[NSTimeZone timeZoneWithAbbreviation:@"JST"]];
            [formatter setDateFormat:@"yyyyMMddHHmmss"];
            NSString *filename = [formatter stringFromDate:[NSDate date]];
            NSString *imageFile = [filename stringByAppendingString:@".png"];
            
            NSString *filePath = [path stringByAppendingPathComponent:imageFile];
            [UIImagePNGRepresentation(image) writeToFile:filePath atomically:YES];
            UnitySendMessage([gameObjectName cStringUsingEncoding:NSUTF8StringEncoding],
                             [callbackMethodName cStringUsingEncoding:NSUTF8StringEncoding],
                             [filePath cStringUsingEncoding:NSUTF8StringEncoding]);
        });
    }];
}

void LobiRec_snap_face_(const char *game_object_name, int  game_object_name_len,
                        const char *callback_method_name, int callback_method_name_len)
{
    NSString *gameObjectName     = make_autorelease_string(game_object_name);
    NSString *callbackMethodName = make_autorelease_string(callback_method_name);
    [LobiRec snapFace:^(UIImage *image) {
#ifdef DEBUG
        NSString *path = [NSString stringWithFormat:KLV_IMAGE_PATH, NSHomeDirectory()];
#else
        NSString *path = [NSString stringWithFormat:KLV_IMAGE_PATH, NSTemporaryDirectory()];
#endif
        
        dispatch_async(dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_DEFAULT, 0), ^{
            
            NSDateFormatter* formatter = [[NSDateFormatter alloc] init];
            NSLocale* local = [[NSLocale alloc] initWithLocaleIdentifier:@"ja_JP"];
            [formatter setLocale:local];
            [formatter setTimeZone:[NSTimeZone timeZoneWithAbbreviation:@"JST"]];
            [formatter setDateFormat:@"yyyyMMddHHmmss"];
            NSString *filename = [formatter stringFromDate:[NSDate date]];
            NSString *imageFile = [filename stringByAppendingString:@".png"];
            
            NSString *filePath = [path stringByAppendingPathComponent:imageFile];
            [UIImagePNGRepresentation(image) writeToFile:filePath atomically:YES];
            UnitySendMessage([gameObjectName cStringUsingEncoding:NSUTF8StringEncoding],
                             [callbackMethodName cStringUsingEncoding:NSUTF8StringEncoding],
                             [filePath cStringUsingEncoding:NSUTF8StringEncoding]);
        });
    }];
}

void LobiRec_is_mic_enabled_(const char *game_object_name, int  game_object_name_len,
                             const char *callback_method_name, int callback_method_name_len)
{
    NSString *gameObjectName     = make_autorelease_string(game_object_name);
    NSString *callbackMethodName = make_autorelease_string(callback_method_name);
    [LobiRec isMicEnabled:^(BOOL enable) {
        UnitySendMessage([gameObjectName cStringUsingEncoding:NSUTF8StringEncoding],
                         [callbackMethodName cStringUsingEncoding:NSUTF8StringEncoding],
                         [(enable ? @"1" : @"0") cStringUsingEncoding:NSUTF8StringEncoding]);
    }];
}

int LobiRec_remove_unretained_video_()
{
    return [LobiRec removeUnretainedVideo] ? 1 : 0;
}

void LobiRec_pause_()
{
    [LobiRec pause];
}

void LobiRec_resume_()
{
    [LobiRec resume];
}

void cameraPreRender_()
{
    [LobiRec prepareFrameForUnity];
}

void onEndOfFrame_()
{
    if ([LobiRec shouldUseOpenGLES]) {
        [LobiRec appendFrameForUnity];
    }
}

void initLobiRec_()
{
    if ([LobiRec shouldUseOpenGLES]) {
        GLint frame;
        glGetIntegerv(GL_FRAMEBUFFER_BINDING, &frame);
        [LobiRec setCurrentContext:[EAGLContext currentContext] withGLView:LobiRec_get_unity_gl_view()];
        [LobiRec createFramebuffer:frame];
    }
    else {
        [LobiRec setCurrentDevice:MTLCreateSystemDefaultDevice()
                        withLayer:(CAMetalLayer*)LobiRec_get_unity_gl_view().layer];
    }
}
