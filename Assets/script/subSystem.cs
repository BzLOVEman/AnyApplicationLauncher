﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System.IO;
using UnityEngine.UI;
using System.Diagnostics;

public class subSystem : MonoBehaviour {

    public string myNum = "01";
    public string myPath;
    public string[] allFiles;
    public string gameName;
    public string[] gamePath;
    public string textString;
    public string[] textPath;
    public string[] videoPath;
    public string videoPathCopy;
    public string videoName;
    private VideoClip videoData;
    private VideoPlayer videoplayer;
    private AudioSource audioSource;
    private Text titleText;
    private Text abstructText;
    private float maskImagePosY;
    private float maskImageHeight;
    private RectTransform maskRect;
    private int i = 0;
    private bool MovieBeBig = false;
    private bool MovieBeSmall = false;
    private GameObject escape;
    private GameObject play;
    private GameObject pose;
    private GameObject prev;
    private GameObject loop;
    private GameObject mute;
    private GameObject sound;
    private GameObject playButton;
    private GameObject scrollTitle;
    private GameObject scrollAbstruct;
    private GameObject back;
    private GameObject prevGame;
    private GameObject nextGame;
    private GameObject videoPlayerObject;
    private GameObject notLoop;

    void Start() {
        /* 各種GameObject初期化 */
        videoPlayerObject=transform.Find("Video Player").gameObject;
        escape=videoPlayerObject.transform.Find("Escape").gameObject;
        escape.GetComponent<Image>().color=new Color(1, 1, 1, 0);
        play=videoPlayerObject.transform.Find("play").gameObject;
        play.GetComponent<Image>().color=new Color(1, 1, 1, 0);
        pose=videoPlayerObject.transform.Find("pose").gameObject;
        pose.GetComponent<Image>().color=new Color(1, 1, 1, 0);
        prev=videoPlayerObject.transform.Find("prev").gameObject;
        prev.GetComponent<Image>().color=new Color(1, 1, 1, 0);
        loop=videoPlayerObject.transform.Find("loop").gameObject;
        loop.GetComponent<Image>().color=new Color(1, 1, 1, 0);
        notLoop=videoPlayerObject.transform.Find("notLoop").gameObject;
        notLoop.GetComponent<Image>().color=new Color(1, 1, 1, 0);
        mute=videoPlayerObject.transform.Find("mute").gameObject;
        mute.GetComponent<Image>().color=new Color(1, 1, 1, 0);
        sound=videoPlayerObject.transform.Find("sound").gameObject;
        sound.GetComponent<Image>().color=new Color(1, 1, 1, 0);
        playButton=transform.Find("PlayButton").gameObject;
        scrollTitle=transform.Find("Scroll View title").gameObject;
        scrollAbstruct=transform.Find("Scroll View abstruct").gameObject;
        back=GameObject.Find("back").gameObject;
        prevGame=GameObject.Find("prevGame").gameObject;
        nextGame=GameObject.Find("nextGame").gameObject;

        /* インポート元フォルダ */
        myPath=Application.dataPath+"/../GameArchives/"+myNum+"/";
        /* exeファイルの検索 */
        gamePath=Directory.GetFiles(@myPath, "*.exe", System.IO.SearchOption.TopDirectoryOnly);
        /* ゲーム名取得 */
        gameName=gamePath[0].Substring(myPath.Length, gamePath[0].Length-4-myPath.Length);
        /* txtファイルの検索 */
        textPath=Directory.GetFiles(@myPath, "*.txt", System.IO.SearchOption.TopDirectoryOnly);
        /* txtファイルデータの読み込み */
        textString=new StreamReader(textPath[0], System.Text.Encoding.GetEncoding("shift_jis")).ReadToEnd();
        /* mp4ファイルの検索 */
        videoPath=Directory.GetFiles(@myPath, "*.mp4", System.IO.SearchOption.TopDirectoryOnly);
        /* mp4ファイル名取得 */
        videoName=videoPath[0].Substring(myPath.Length, videoPath[0].Length-myPath.Length);
        /* video player取得 */
        videoplayer=videoPlayerObject.GetComponent<VideoPlayer>();
        /* videoのpath設定 */
        videoplayer.url=videoPath[0];
        /* video再生 */
        audioSource=videoPlayerObject.GetComponent<AudioSource>();
        audioSource.mute=true;
        videoplayer.isLooping=true;
        videoplayer.Play();

        /* データ上書き */
        titleText=scrollTitle.
                    gameObject.transform.Find("Viewport").
                    gameObject.transform.Find("Content").
                    gameObject.transform.Find("titleText").
                    gameObject.GetComponent<Text>();
        titleText.text=gameName;
        abstructText=scrollAbstruct.
                    gameObject.transform.Find("Viewport").
                    gameObject.transform.Find("Content").
                    gameObject.transform.Find("adstructText").
                    gameObject.GetComponent<Text>();
        abstructText.text=textString;

        /* movie用 */
        maskRect=videoPlayerObject.gameObject.transform.Find("MaskImage").gameObject.GetComponent<RectTransform>();
        maskImagePosY=maskRect.position.y;
        maskImageHeight=maskRect.sizeDelta.y;
    }

    void Update() {
        if (MovieBeBig) {
            bigMovie();
        }
        if (MovieBeSmall) {
            smallMovie();
        }
        changeMultipleButton();
    }

    public void GamePlayButton() {
        Process.Start(@gamePath[0]);
    }

    public void MoviePlayButton() {
        MovieBeBig=true;
    }

    private void bigMovie() {
        //大きく
        if (maskRect.localPosition.y>=15) {
            //1080-maskImageHeight -> 事実大きくした高さの量
            //中心点の二倍速で大きさが変わらなければならない
            //
            maskRect.localPosition=new Vector3(0, maskRect.localPosition.y-( 1080-maskImageHeight )/3/30f, 0);
        } else {
            maskRect.localPosition=Vector3.zero;
        }
        if (1080-maskRect.sizeDelta.y>=25) {
            maskRect.sizeDelta=new Vector2(maskRect.sizeDelta.x, maskRect.sizeDelta.y+( 1080-maskImageHeight )/3/30f*2);
        } else {
            maskRect.sizeDelta=new Vector2(1920, 1080);
            MovieBeBig=false;
        }
        //UI表示
        if (!MovieBeBig) {
            escape.GetComponent<Image>().color=new Color(1, 1, 1, 1);
            play.GetComponent<Image>().color=new Color(1, 1, 1, 1);
            pose.GetComponent<Image>().color=new Color(1, 1, 1, 1);
            prev.GetComponent<Image>().color=new Color(1, 1, 1, 1);
            loop.GetComponent<Image>().color=new Color(1, 1, 1, 1);
            notLoop.GetComponent<Image>().color=new Color(1, 1, 1, 1);
            mute.GetComponent<Image>().color=new Color(1, 1, 1, 1);
            sound.GetComponent<Image>().color=new Color(1, 1, 1, 1);
        }

        //このボタンを消す
        videoPlayerObject.gameObject.transform.Find("movieUpButton").GetComponent<Button>().interactable=false;
        videoPlayerObject.gameObject.transform.Find("movieUpButton").GetComponent<Image>().raycastTarget=false;
        //画面UI消す
        playButton.SetActive(false);
        scrollTitle.SetActive(false);
        scrollAbstruct.SetActive(false);
        back.GetComponent<Image>().enabled=false;
        prevGame.GetComponent<Image>().enabled=false;
        nextGame.GetComponent<Image>().enabled=false;

        //音を付ける
        audioSource.mute=false;
    }

    private void smallMovie() {
        //小さく
        if (maskRect.localPosition.y<maskImagePosY-15) {
            //1080-maskImageHeight -> 事実大きくした高さの量
            //中心点の二倍速で大きさが変わらなければならない
            //
            maskRect.localPosition=new Vector3(0, maskRect.localPosition.y+( 1080-maskImageHeight )/3/30f, 0);
        } else {
            maskRect.localPosition=new Vector3(0,maskImagePosY,0);
        }
        if (maskRect.sizeDelta.y - maskImageHeight>=25) {
            maskRect.sizeDelta=new Vector2(maskRect.sizeDelta.x, maskRect.sizeDelta.y-( 1080-maskImageHeight )/3/30f*2);
        } else {
            maskRect.sizeDelta=new Vector2(1920, maskImageHeight);
            MovieBeSmall=false;
        }
        //UI表示消す
        if (MovieBeSmall) {
            escape.GetComponent<Image>().color=new Color(1, 1, 1, 0);
            play.GetComponent<Image>().color=new Color(1, 1, 1, 0);
            pose.GetComponent<Image>().color=new Color(1, 1, 1, 0);
            prev.GetComponent<Image>().color=new Color(1, 1, 1, 0);
            loop.GetComponent<Image>().color=new Color(1, 1, 1, 0);
            notLoop.GetComponent<Image>().color=new Color(1, 1, 1, 0);
            mute.GetComponent<Image>().color=new Color(1, 1, 1, 0);
            sound.GetComponent<Image>().color=new Color(1, 1, 1, 0);
        }
        if (!MovieBeSmall) {
            //このボタンを出す
            videoPlayerObject.gameObject.transform.Find("movieUpButton").GetComponent<Button>().interactable=true;
            videoPlayerObject.gameObject.transform.Find("movieUpButton").GetComponent<Image>().raycastTarget=true;
            //画面UI出す
            playButton.SetActive(true);
            scrollTitle.SetActive(true);
            scrollAbstruct.SetActive(true);
            back.GetComponent<Image>().enabled=true;
            prevGame.GetComponent<Image>().enabled=true;
            nextGame.GetComponent<Image>().enabled=true;
            //音を消す
            audioSource.mute=true;
        }
    }

    public void OnEscapeButton() {
        if (escape.GetComponent<Image>().color.a>=1) {
            MovieBeSmall=true;
        }
    }

    public void OnPlayButton() {
        if (play.GetComponent<Image>().color.a>=1) {
            videoplayer.Play();
        }
    }

    public void OnPoseButton() {
        if (pose.GetComponent<Image>().color.a>=1) {
            videoplayer.Pause();
        }
    }

    public void OnPrevButton() {
        if (prev.GetComponent<Image>().color.a>=1) {
            videoplayer.Stop();
            videoplayer.Play();
        }
    }

    public void OnLoopButton() {
        if (loop.GetComponent<Image>().color.a>=1) {
            videoplayer.isLooping=!videoplayer.isLooping;
        }
    }

    public void OnMuteButton() {
        if (mute.GetComponent<Image>().color.a>=1) {
            audioSource.mute=false;
        }
    }

    public void OnSoundButton() {
        if (sound.GetComponent<Image>().color.a>=1) {
            audioSource.mute=true;
        }
    }

    public void changeMultipleButton() {
        if (videoplayer.isPlaying) {
            play.SetActive(false);
            pose.SetActive(true);
        } else {
            play.SetActive(true);
            pose.SetActive(false);
        }

        if (!audioSource.mute) {
            mute.SetActive(false);
            sound.SetActive(true);
        } else {
            mute.SetActive(true);
            sound.SetActive(false);
        }

        if (videoplayer.isLooping) {
            notLoop.SetActive(false);
        } else {
            notLoop.SetActive(true);
        }
    }


}