using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CustomImporter : AssetPostprocessor
{
    public static MyImportSettings settingData;
    private void OnPreprocessTexture()
    {
        CheckSetting(MyImportType.Texture);
    }

    private void OnPreprocessAudio()
    {
        CheckSetting(MyImportType.Audio);
    }

    private void CheckSetting(MyImportType type)
    {
        settingData ??= AssetDatabase.LoadAssetAtPath<MyImportSettings>("Assets/Tools/Config/MyImportSettings.asset");
        if (assetImporter.importSettingsMissing)//导入时
        {
            
            switch (type)
            {
                case MyImportType.Texture:
                    SetTexture();
                    break;
                case MyImportType.Audio:
                    SetAudio();
                    break;
                default:
                    break;
            }
        }
        
    }

    private void SetTexture()
    {
        var importer = (TextureImporter)assetImporter;
        var path = importer.assetPath;
        TextureImportData mySetting = null;
        if (settingData != null)
        {
            var data = settingData.textureData;
            if (data != null)
            {
                for (var i = 0; i < data.Count; i++)
                {
                    if (path.StartsWith(data[i].path))
                    {
                        mySetting = data[i];
                        break;
                    }
                }
            }
            
        }
        //没有配置文件
        if (mySetting == null)
        {
            mySetting = new TextureImportData();
            mySetting.isReadable = false;
            mySetting.maxSize = 2048;
            mySetting.mipmapEnabled = false;
            mySetting.androidFormat = (MyTextureFormatType)TextureImporterFormat.RGBA32;
            mySetting.iOSFormat = (MyTextureFormatType)TextureImporterFormat.RGBA32;
        }
        
        importer.textureType = mySetting.textureImporterType;
        importer.isReadable = mySetting.isReadable;
        importer.mipmapEnabled = mySetting.mipmapEnabled;
        importer.GetSourceTextureWidthAndHeight(out var width, out var height);
        if (width > mySetting.maxSize || height >  mySetting.maxSize )
        {
            Debug.LogError($"{assetPath} size is too big", importer);
        }
        
        if(width%4 !=0 || height%4 !=0)
        {
            Debug.LogError($"{assetPath} size is not multiple of 4", importer);
        }

        var androidSetting = importer.GetPlatformTextureSettings("Android");
        androidSetting.overridden = true;
        androidSetting.format = (TextureImporterFormat)mySetting.androidFormat ;
        androidSetting.compressionQuality = 50 ;
        importer.SetPlatformTextureSettings(androidSetting);
                    
        var iosSetting = importer.GetPlatformTextureSettings("iPhone");
        iosSetting.overridden = true;
        iosSetting.format = (TextureImporterFormat)mySetting.iOSFormat;
        iosSetting.compressionQuality = 50 ;
        importer.SetPlatformTextureSettings(iosSetting);
    }
    
    
    private void SetAudio()
    {
        var importer = (AudioImporter)assetImporter;
        var path = importer.assetPath;
        AudioImportData mySetting = null;
        if (settingData != null)
        {
            var data = settingData.audioData;
            if (data != null)
            {
                foreach (var t in data.Where(t => path.StartsWith(t.path)))
                {
                    mySetting = t;
                    break;
                }
            }
        }
        
        
        importer.ambisonic = false;
    }


    #region 音效建议配置
    
    //-------------------------------------
    // forceToMono 强制单声道，这样的好处是减少音效文件的内存占用，一般在手机游戏都是关闭的，因为在不带耳机的情况下是听不出来单声道和双声道区别的。
    // 建议设置，大于2秒的音效文件全部关闭双声道。一般勾选了强制单声道之后也要勾选Normalize选项，这样引擎内部会选择最合适的方式来强行转换单声道。

    // ambisonic 这个是环境音，一般手机项目都不勾选的
    
    // loadInBackground 在后台加载异步，这样做可以使得声音的加载不阻塞主线程。但是他也有坏处，如果文件过大就会导致加载时间过长，而在第一次播放的时候，声音和画面会有不同步的问题。
    // 建议设置：大于10秒的文件，一般是背景声音、对话语音等，全部不勾选，其他一律勾选。
     
    //-------------------------------------
    // Load Type 加载音频的方式。
    // Decompress On Load 表示加载完音频文件之后，无压缩的释放到内存内，这样做的好处是播放的时候无需解压，速度快，减少CPU的开销，坏处是占用较多的内存。
    // 建议设置：小于2秒的选用此选项。
    // Compress In Memory 表示加载完音频文件之后，以压缩的方式放到内存中，这样做的好处是节省了内存，坏处是播放的时候会消耗CPU进行解压处理。
    // 建议设置：一般大于2秒，小于10秒的文件选择这个选项。
    // Streaming 播放音频的时候流式加载，好处是文件不占用内存，坏处是加载的时候对IO、CPU都会有开销。
    // 建议设置：一般对大于10秒的文件才会勾选此选项。
    
    //-------------------------------------
    // Preload Audio Data 预加载音效数据，这个是在进入场景的时候进行预加载的，会占用内存，
    // 建议设置：一般对大于10秒的文件都不进行预加载，除非有特殊情况。知道这个文件在这个场景内肯定会用到，需要提前进行预加载。其他文件都勾选预加载
    
    //-------------------------------------
    // Compression Format 压缩格式，这是指音频文件的压缩格式。
    // PCM，最高质量和最大文件的方式。
    // 建议设置：是小于2秒的文件。
    // Vorbis/Mp3 低质量的，压缩更小的文件，压缩率可以在选择了Vorbis格式之后，在Quality中进行选择，值越小，压缩越厉害，文件也越小。
    // 建议设置：70。我们项目的设置是大于5秒的文件选择Vorbis
    // ADPCM，是介于PCM和Vorbis之间的压缩格式，官方推荐一些包含噪声且被多次播放的音效文件例如脚步声、打击声、武器碰撞声等可以选择
    // 建议设置：是2-5秒的文件选用此选项。
    
    
    
    
    // 建议----2
    
    // 1. 经常播放的声音
    //     大量播放的声音（例如武器声音、脚步声、撞击声等）。
    //
    // 最好使用以下设置（也适合 10 秒以下的短声音）：
    //
    // 加载类型：加载时解压
    //
    //     压缩格式：ADPCM
    //
    // （来自 Unity 文档） 加载时解压缩：音频文件在加载后将立即解压缩。对于较小的压缩声音使用此选项，以避免动态解压缩的性能开销。请注意，在加载时解压缩 Vorbis 编码的声音将使用比保持压缩状态多十倍的内存（对于 ADPCM 编码，约为 3.5 倍），因此不要对大文件使用此选项。
    //
    // （来自 Unity 文档） ADPCM：此格式对于包含相当多噪音且需要大量播放的声音非常有用，例如脚步声、撞击声、武器声。压缩比比 PCM 小 3.5 倍，但 CPU 占用率比 MP3/Vorbis 格式低得多，这使其成为上述声音类别的首选。
    //
    // 2. 周期性或罕见的演奏声音
    //     不需要频繁播放的声音，例如，回合开始时播音员的声音、赛车游戏开始时的计时器声音，或者基本上任何超过 10 秒 但低于 10 秒 的声音1 分钟。
    //
    // 加载类型：压缩在内存中
    //
    //     压缩格式：ADPCM
    //
    // （来自 Unity 文档） 压缩在内存中：将声音压缩在内存中并在播放时解压缩。此选项会产生轻微的性能开销（尤其是对于 Ogg/Vorbis 压缩文件），因此仅将其用于较大的文件，因为在加载时解压缩会使用大量内存。解压缩发生在混音器线程上，可以在分析器窗口的音频窗格中的 "DSP CPU" 部分进行监视。
    //
    // 3. 背景/环境声音
    //     背景/环境声音，长度超过一分钟。
    //
    // 加载类型：Streaming（如果您的目标是 WebGL，则为内存压缩）
    //
    // 压缩格式：Vorbis
    #endregion
    
    
    // 所有资源导入完成后调用
    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
        string[] movedFromAssetPaths)
    {
        foreach (var asset in importedAssets)
        {
            if (asset.EndsWith(".mp3") || asset.EndsWith(".wav"))
            { 
                var audio = AssetDatabase.LoadAssetAtPath<AudioClip>(asset);
                var importer = AssetImporter.GetAtPath(asset) as AudioImporter;
                if (importer != null)
                {
                    
                    
                    if (settingData != null)
                    {
                        var data = settingData.audioData;
                        if (data != null)
                        {
                            foreach (var t in data.Where(t => asset.StartsWith(t.path)))
                            {
                                if (audio.length < t.minTime || audio.length > t.maxTime)
                                {
                                    Debug.LogError($"音频文件{asset}时长不符合建议要求，应该在{t.minTime}到{t.maxTime}之间,当前时长为{audio.length}",importer);
                                }
                                break;
                            }
                        }
                    }
                    
                    
                    importer.forceToMono = audio.length > 2;
                    importer.ambisonic = false;
                    importer.loadInBackground = audio.length <= 10;
                    
                    var audioCompressionFormat = audio.length switch
                    {
                        <= 2 => AudioCompressionFormat.PCM,
                        <= 5 => AudioCompressionFormat.ADPCM,
                        _ => AudioCompressionFormat.Vorbis
                    };
                    
                    var loadType = audio.length switch
                    {
                        <= 2 => AudioClipLoadType.DecompressOnLoad,
                        <= 10 => AudioClipLoadType.CompressedInMemory,
                        _ => AudioClipLoadType.Streaming
                    };
                    
                    var sampleSetting = new AudioImporterSampleSettings
                    {
                        loadType = loadType,
                        preloadAudioData = audio.length <= 10,
                        compressionFormat = audioCompressionFormat,
                        sampleRateSetting = AudioSampleRateSetting.OverrideSampleRate,
                        quality = 70,
                        sampleRateOverride = 44100
                    };
                    importer.SetOverrideSampleSettings("Standalone", sampleSetting);
                    importer.SetOverrideSampleSettings("Android", sampleSetting);
                    importer.SetOverrideSampleSettings("iOS", sampleSetting);
                }
            }
        }
    }
    
   
}
