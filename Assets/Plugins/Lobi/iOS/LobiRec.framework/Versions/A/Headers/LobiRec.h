//
//  LobiRec.h
//  LobiRankingSample
//
//  Created by takahashi-kohei on 2014/03/14.
//  Copyright (c) 2014年 KAMEDAkyosuke. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <OpenGLES/ES1/glext.h>
#import <OpenGLES/ES2/glext.h>
#import <UIKit/UIKit.h>
#import <AVFoundation/AVFoundation.h>

#import "LobiRecConst.h"

/**
 ゲーム録画エンジンクラス。シングルトンでインスタンスを提供します。
 
 OpenGLコンテキストとviewを受け、フレームバッファに描画されたゲームを動画ファイルとして保存します。
 ワイプの表示 / マイク入力 / マイクボリューム / ゲームボリューム / モザイク処理 などの制御を各プロパティにて行います。
 
 - ゲーム録画エンジンから送出される通知と通知名称
 - KLVDryingUpInStorageNotification
 ゲーム録画ファイルの保存領域が200MB以下になった際に録画処理が停止され、userInfoにnilを格納して通知する通知名称です。
 
 - KLVMovieCreatedNotification
 ゲーム録画情報をサーバにPOSTした際に、userInfoに"url"をキーとしたhttp://play.lobi.coのプレビューURLを格納して通知する通知名称です。
 また"videoId"をキーとし、アップロードした動画のIDが格納されています。

 - KLVMovieCreatedErrorNotification
 ゲーム録画情報をサーバにPOST中エラーが発生した際に、userInfoにnilを格納して通知する通知名称です。
 
 - KLVMovieUploadedNotification
 ゲーム録画ファイルをサーバにPOSTした際に、userInfoにnilを格納して通知する通知名称です。
 
 - KLVMovieUploadedErrorNotification;
 ゲーム録画ファイルをサーバにPOSTエラーが発生した際に、userInfoにnilを格納して通知する通知名称です。
 
 - KLVDismissingPostVideoViewControllerNotification;
 動画ポストviewControllerを閉じた時に、userInfoに動画のポスト処理の有無を格納して通知する通知名称です。
 
 - KLVMicEnableErrorNotification;
 マイク録音をYESに設定した際にプライバシーにより設定できなかった場合に、userInfoにnilを格納して通知する通知名称です。
 */

@interface LobiRec : NSObject

+ (instancetype)sharedInstance;
+ (NSString*)SDKVersion;

/**
 *  OpenGLESモードで動作すべきかの確認
 *
 *  @return OpenGLESモードの場合はYES
 */
+ (BOOL)shouldUseOpenGLES;

/**
 *  明示的にMetalを使うことを通知
 */
+ (void)useMetal;

/**
 *  明示的にOpenGLESを使うことを通知
 */
+ (void)useOpenGLES;

/**
 *  録画中、録画停止中を提供します。
 */
@property (nonatomic, readonly) BOOL isCapturing;

/**
 *  録画中に表示するワイプの状態を設定します。
 */
@property (nonatomic, assign) KLVLiveWipeStatus liveWipeStatus;

/**
 *  録画中に表示するワイプのx座標を設定します。デバイス毎に設定する必要があります。
 */
@property (nonatomic, assign) CGFloat           wipePositionX;

/**
 *  録画中に表示するワイプのy座標を設定します。デバイス毎に設定する必要があります。
 */
@property (nonatomic, assign) CGFloat           wipePositionY;

/**
 *  録画中に表示するワイプのサイズを設定します。
 */
@property (nonatomic, assign) CGFloat           wipeSquareSize;

/**
 *  録画中のゲームボリュームの大きさを設定します。(0.0 - 1.0)
 */
@property (nonatomic, assign) CGFloat           gameSoundVolume;

/**
 *  アフレコ録画中のボリュームの大きさを設定します。(0.0 - 1.0)
 *  値を入力しても何も行われません。また、常に0.0が取得できます。
 */
@property (nonatomic, assign) CGFloat           afterRecordingVolume __attribute__ ((deprecated));

/**
 *  録画中のマイク入力ボリュームの大きさを設定します。(0.0 - 1.0)
 */
@property (nonatomic, assign) CGFloat           micVolume;

/**
 *  録画中のマイク入力可否を設定します。
 */
@property (nonatomic, assign) BOOL              micEnable;

/**
 *  録画中このプロパティがtrueの間、ゲーム録画ファイルにモザイク処理がかかります。
 */
@property (nonatomic, assign) BOOL              preventSpoiler;

/**
 *  録画中このプロパティがtrueの間、ゲーム画面にワイプの表示がされません。
 */
@property (nonatomic, assign) BOOL              hideFaceOnPreview;

/**
 　レンダリングループのフレームに対してゲーム録画ファイルに出力する回数を設定します。
 
 ex.
 
 capturePerFrame = 1 : 毎フレーム出力
 
 capturePerFrame = 4 : 4フレーム毎に1回出力
 */
@property (nonatomic, assign) NSUInteger        capturePerFrame;

/**
 Unityエンジンを使用する場合に利用します。このコールバックのパラメータから取得できるフレームバッファをgDefaultFBOに設定します。
 */
@property (nonatomic, copy) void(^activeFramebufferCallback)(GLuint) __attribute__ ((deprecated));

/**
 * 本クラスに設定されたコンテキストを返します。
 */
@property (nonatomic, readonly) EAGLContext *context;

/**
 * 録画中にアプリサスペンド時、アプリが復帰した場合に録画を自動的に再開します。
 */
@property (nonatomic, assign) BOOL stickyRecording;


/**
 * 録画中にインジケータを表示します。
 */
@property (nonatomic, assign) BOOL recordingIndicator;

@property (nonatomic, assign) BOOL canAfterRecording;

@property (nonatomic, readonly) BOOL recorderSwitch;

/**
 *  OpenGLコンテキストとviewをゲーム録画エンジンに設定します。
 *
 *  @param context ゲームにて生成したコンテキストを設定します。
 *  @param glView  ゲームにて生成した描画先のviewを設定します。
 */
+ (void)setCurrentContext:(EAGLContext*)context withGLView:(UIView*)glView;

/**
 *  ゲームにて生成した録画対象となるフレームバッファを設定します。
 *
 *  @param framebufferRef
 */
+ (void)createFramebuffer:(GLuint)framebufferRef;

/**
 *  ゲーム録画フレームバッファを用意します。
 *
 *  @return ゲーム録画フレームバッファの用意に成功した場合trueを返します。
 */
+ (BOOL)prepareFrame;

/**
 *  ゲームにて生成した録画対象となるフレームバッファからゲーム録画ファイルに出力します。
 *
 *  @param framebufferRef ゲームにて生成した録画対象となるフレームバッファ
 *
 *  @return フレームバッファをゲーム録画ファイルに出力成功した場合trueを返します。
 */
+ (BOOL)appendFrame:(GLuint)framebufferRef;

/**
 *  ゲーム録画開始処理。ワイプ / マイク設定 / キャプチャフレーム設定は本処理より前に設定する必要があります。
 */
+ (void)startCapturing;

/**
 *  ゲーム録画終了処理
 */
+ (void)stopCapturing;

/**
 *  ゲーム録画終了処理。完了ハンドラあり。
 */
+ (void)stopCapturingWithHandler:(void(^)(void))completion;

/**
 *  ゲーム録画ファイルが存在している場合trueを返します。
 *
 *  @return ゲーム録画ファイルが存在している場合trueを返します。
 */
+ (BOOL)hasMovie;

/**
 * LobiPlayサイトを表示します。
 */
+ (void)presentLobiPlay __attribute__ ((deprecated));

/**
 * LobiPlayサイトを表示します。
 *
 *  @param prepareHandler 表示完了後に行う処理を指定します。バックグラウンドからの復帰時にも呼び出されます
 *  @param afterHandler   dissmiss完了後に行う処理を指定します
 */
+ (void)presentLobiPlay:(void(^)(void))prepareHandler
           afterHandler:(void(^)(void))afterHandler;

/**
 * 動画IDを指定してLobiPlayサイトを表示します。
 *
 *  @param videoId        表示する動画のID
 *  @param prepareHandler 表示完了後に行う処理を指定します。バックグラウンドからの復帰時にも呼び出されます
 *  @param afterHandler   dissmiss完了後に行う処理を指定します
 */
+ (void)presentLobiPlay:(NSString*)videoId
         prepareHandler:(void(^)(void))prepareHandler
           afterHandler:(void(^)(void))afterHandler;

/**
 * ユーザやカテゴリ、メタ情報を指定してLobiPlayサイトを表示します。
 *
 *  @param userExId       ユーザのExID
 *  @param category       カテゴリ。カテゴリは[Lobi Developer](https://developer.lobi.co/ja)サイト[アプリ管理ページ](https://developer.lobi.co/myapps)にて追加されたカテゴリIDを設定します。カテゴリとは投稿された動画をゲームステージなどでカテゴリ分けして表示するための機能です。
 *  @param letsPlay       実況の動画かどうかを指定します
 *  @param metaFields     メタ情報
 *  @param prepareHandler 表示完了後に行う処理を指定します。バックグラウンドからの復帰時にも呼び出されます
 *  @param afterHandler   dissmiss完了後に行う処理を指定します
 */
+ (void)presentLobiPlay:(NSString*)userExId
               category:(NSString*)category
               letsPlay:(BOOL)letsPlay
             metaFields:(NSString*)metaFields
         prepareHandler:(void(^)(void))prepareHandler
           afterHandler:(void(^)(void))afterHandler;

/**
 * イベントを指定してLobiPlayサイトを表示します。
 *
 *  @param eventFields    イベント
 */
+ (void)presentLobiPlayWithEventFields:(NSString*)eventFields __attribute__ ((deprecated));

/**
 * イベントを指定してLobiPlayサイトを表示します。
 *
 *  @param eventFields    イベント
 *  @param prepareHandler 表示完了後に行う処理を指定します。バックグラウンドからの復帰時にも呼び出されます
 *  @param afterHandler   dissmiss完了後に行う処理を指定します
 */
+ (void)presentLobiPlayWithEventFields:(NSString*)eventFields
                        prepareHandler:(void(^)(void))prepareHandler
                          afterHandler:(void(^)(void))afterHandler;

/**
 *  動画のポスト画面を表示します。
 *
 *  @param title シェアタイトル
 *  @param postDescrition コメント
 *  @param postScore スコア
 *  @param postCategory カテゴリには[Lobi Developer](https://developer.lobi.co/ja)サイト[アプリ管理ページ](https://developer.lobi.co/myapps)にて追加されたカテゴリIDを設定します。カテゴリとは投稿された動画をゲームステージなどでカテゴリ分けして表示するための機能です。
 *  @param prepareHandler 表示完了後に行う処理を指定します。バックグラウンドからの復帰時にも呼び出されます
 *  @param afterHandler   dissmiss完了後に行う処理を指定します
 */
+ (void)presentLobiPostWithTitle:(NSString *)title
                  postDescrition:(NSString *)postDescrition
                       postScore:(int64_t)postScore
                    postCategory:(NSString *)postCategory
                  prepareHandler:(void(^)(void))prepareHandler
                    afterHandler:(void(^)(void))afterHandler;

/**
 *  動画のポスト画面を表示します。
 *
 *  @param title シェアタイトル
 *  @param postDescrition コメント
 *  @param postScore スコア
 *  @param postCategory カテゴリには[Lobi Developer](https://developer.lobi.co/ja)サイト[アプリ管理ページ](https://developer.lobi.co/myapps)にて追加されたカテゴリIDを設定します。カテゴリとは投稿された動画をゲームステージなどでカテゴリ分けして表示するための機能です。
 *  @param postMetaData メタ情報
 *  @param prepareHandler 表示完了後に行う処理を指定します。バックグラウンドからの復帰時にも呼び出されます
 *  @param afterHandler   dissmiss完了後に行う処理を指定します
 */
+ (void)presentLobiPostWithTitle:(NSString*)title
                  postDescrition:(NSString*)postDescrition
                       postScore:(int64_t)postScore
                    postCategory:(NSString*)postCategory
                    postMetaData:(NSString*)postMetaData
                  prepareHandler:(void(^)(void))prepareHandler
                    afterHandler:(void(^)(void))afterHandler;


/**
 * ゲーム録画中に画面のスクリーンショットを取得することができます。
 * @param handler UIImageを返却します。
 */
+ (void)snap:(void(^)(UIImage*))handler;


/**
 * ゲーム実況録画中に顔のスクリーンショットを取得することができます。
 * @param handler UIImageを返却します。
 */
+ (void)snapFace:(void(^)(UIImage*))handler;


/**
 * マイク録音がプライバシーにおいて有効か無効かを返します。
 * @param handler micEnabledを返却します。
 */
+ (void)isMicEnabled:(void(^)(BOOL))handler;

/**
 * 録画ポーズ状態を取得します
 * @deprecated `+ (BOOL)isPaused` を使用してください。
 * @return ポーズ中の場合YESを返します。
 */
+ (BOOL)isPause     __attribute__ ((deprecated("use isPaused")));

/**
 * 録画ポーズ状態を取得します
 * @return ポーズ中の場合YESを返します。
 */
+ (BOOL)isPaused;

/**
 * 録画のポーズを行います。startCapturingを行うことでポーズが解除されます。
 */
+ (void)pause;

/**
 * 録画のポーズの解除を行います。
 */
+ (void)resume;



+ (BOOL)prepareFrameForUnity;
+ (BOOL)appendFrameForUnity;

+ (void)setRecorderSwitch:(BOOL)recorderSwitch;
+ (BOOL)removeUnretainedVideo;

/**
 * 独自のオーディオをデータを録音するためのフォーマットを設定します。
 */
+ (void)setupInputAudioStreamBasicDescription:(AudioStreamBasicDescription*)audioStreamBasicDescription;

/**
 * 独自のオーディオのデータを書き込みます
 */
+ (void)writeInputAudioData:(NSData*)data;

/**
 * 独自のオーディオのバイト列を書き込みます
 */
+ (void)writeInputAudioBytes:(void*)bytes length:(UInt32)length;

@end

