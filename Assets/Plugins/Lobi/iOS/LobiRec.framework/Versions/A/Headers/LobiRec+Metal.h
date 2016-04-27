//
//  LobiRec+Metal.h
//  LobiCore
//
//  Created by hiruma-kazuya on 2015/05/22.
//  Copyright (c) 2015年 面白法人カヤック. All rights reserved.
//

#import "LobiRec.h"

#import <Metal/Metal.h>

/**
 *  ゲーム録画エンジンクラスのMetal版。
 *  OpenGL版の各種メソッドをMetal用に変更します。
 */
@interface LobiRec (Metal)

/**
 *  ゲーム画面キャプチャ用のコマンドバッファを追加します。
 *
 *  @param commandBuffer キャプチャ対象のコマンドバッファを渡します。
 */
+ (void)addMetalCommands:(id <MTLCommandBuffer>)commandBuffer;

/**
 *  現在のMTLDeviceと描画先のCAMetalLayerを設定します。
 *
 *  @param device     現在のMTLDevice
 *  @param metalLayer レンダリング先レイヤー
 */
+ (void)setCurrentDevice:(id <MTLDevice>)device withLayer:(CAMetalLayer *)metalLayer;

/**
 *  現在のdrawbleを設定
 */
+ (void)setCurrentDrawable:(id <CAMetalDrawable>)drawble;

@end
