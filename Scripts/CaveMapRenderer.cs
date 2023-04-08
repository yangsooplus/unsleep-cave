using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveMapRenderer : MonoBehaviour
{
    public SpriteRenderer background;
    public SpriteRenderer black;
    public Sprite[] backgroundSprites;

    public GameObject[] holes;

    public AudioSource leftAudio;
    public AudioSource rightAudio;
    public AudioSource defaultAudio;
    public AudioClip[] audioClips;

    public AudioClip walkClip;
    public bool moving = false;


    public void proceed(Cavern cavern)
    {
        defaultAudio.Play();
        StartCoroutine(proceedCavern(cavern));
    }

    public void back(Cavern cavern)
    {
        defaultAudio.Play();
        StartCoroutine(backCavern(cavern));
    }

    public void renderCavern(Cavern cavern)
    {

        // 구멍 다 숨기기
        foreach (GameObject h in holes)
        {
            h.SetActive(false);
        }

        // <-------------- 배경 ---------------->

        if (cavern.routeCnt < 0) 
            return; 
        else if (cavern.routeCnt == 999)
        {

            background.sprite = backgroundSprites[4]; // 마지막 방 배경
            StartCoroutine(fadeOutCavernAudio());
            return;
        }

        background.sprite = backgroundSprites[cavern.routeCnt];

        if (cavern.next.Length > 0)
            holes[cavern.routeCnt].SetActive(true);


        // <-------------- 소리 ---------------->

        switch (cavern.soundPosition)
        {
            case "LR":
                setCarvenAudioLR(cavern.soundIndex, cavern.volume, cavern.soundIndex2, cavern.volume2);
                break;
            case "C":
                setCavernAudio(leftAudio, cavern.soundIndex, cavern.volume, true);
                setCavernAudio(rightAudio, cavern.soundIndex, cavern.volume, true);
                break;
            case "L":
                muteAudioSource(rightAudio);
                setCavernAudio(leftAudio, cavern.soundIndex, cavern.volume, true);
                break;
            case "R":
                muteAudioSource(leftAudio);
                setCavernAudio(rightAudio, cavern.soundIndex, cavern.volume, true);
                break;
            case "c":
                setCavernAudio(leftAudio, cavern.soundIndex, cavern.volume, false);
                setCavernAudio(rightAudio, cavern.soundIndex, cavern.volume, false);
                break;
            case "l":
                setCavernAudio(leftAudio, cavern.soundIndex, cavern.volume, false);
                muteAudioSource(rightAudio);
                break;
            case "r":
                setCavernAudio(rightAudio, cavern.soundIndex, cavern.volume, false);
                muteAudioSource(leftAudio);
                break;
        }
    }

    IEnumerator proceedCavern(Cavern cavern)
    {
        moving = true;
        yield return StartCoroutine(proceedEffect());
        moving = false;
        renderCavern(cavern);
    }


    IEnumerator backCavern(Cavern cavern)
    {
        moving = true;
        yield return StartCoroutine(backEffect());
        moving = false;
        renderCavern(cavern);
    }


    private void setCavernAudio(AudioSource audioSource, int idx, float vol, bool first)
    {
        if (audioSource.mute)
            audioSource.mute = false;

        if (first)
        {
            leftAudio.clip = audioClips[idx];
            leftAudio.Play();
            rightAudio.clip = audioClips[idx];
            rightAudio.Play();
        }
           
        audioSource.volume = vol;
    }

    private void setCarvenAudioLR(int idx, float vol, int idx2, float vol2)
    {
        if (leftAudio.mute)
            leftAudio.mute = false;

        leftAudio.clip = audioClips[idx];
        leftAudio.volume = vol;


        if (rightAudio.mute)
            rightAudio.mute = false;

        rightAudio.clip = audioClips[idx2];
        rightAudio.volume = vol2;
    }

    private void muteAudioSource(AudioSource audioSource)
    {
        audioSource.mute = true;
    }

    IEnumerator proceedEffect()
    {
        StartCoroutine(shakeCavern(0.3f));
        StartCoroutine(scaleCavern(1f, 1.1f, 6f));
        yield return StartCoroutine(fadeInOut(1f, 1f));

        StartCoroutine(fadeInOut(0f, 0.5f));
        transform.position = new Vector3(0f, 0f, 0f);
        transform.localScale = new Vector3(1f, 1f, 1f);
    }

    IEnumerator backEffect()
    {
        StartCoroutine(scaleCavern(1f, 0.9f, 6f));
        yield return StartCoroutine(fadeInOut(1f, 1f));

        StartCoroutine(fadeInOut(0f, 0.5f));
        transform.position = new Vector3(0f, 0f, 0f);
        transform.localScale = new Vector3(1f, 1f, 1f);
    }

    IEnumerator fadeOutCavernAudio()
    {
        float rightvol = rightAudio.volume;
        float leftvol = leftAudio.volume;

        while (rightvol > 0f && leftvol > 0f)
        {

            rightvol -= 0.1f * Time.deltaTime;
            leftvol -= 0.1f * Time.deltaTime;

            rightAudio.volume = rightvol;
            leftAudio.volume = leftvol;

            yield return null;
        }
    }

    IEnumerator fadeInOut(float target, float time)
    {
        float alpha = black.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / time)
        {
            Color newColor = new Color(0, 0, 0, Mathf.Lerp(alpha, target, t));
            black.color = newColor;
            yield return null;
        }
    }

    IEnumerator shakeCavern(float time)
    {
        
        for (float y = 0.0f; y < 0.5f; y += Time.deltaTime / time)
        {
            Vector3 pos = transform.position;
            transform.position = new Vector3(pos.x, y, pos.z);
            yield return null;
        }

        for (float y = 0.5f; y > -0.5f; y -= Time.deltaTime / time)
        {
            Vector3 pos = transform.position;
            transform.position = new Vector3(pos.x, y, pos.z);
            yield return null;
        }

        for (float y = -0.5f; y < 0.5f; y += Time.deltaTime / time)
        {
            Vector3 pos = transform.position;
            transform.position = new Vector3(pos.x, y, pos.z);
            yield return null;
        }

        for (float y = 0.5f; y > -0.5f; y -= Time.deltaTime / time)
        {
            Vector3 pos = transform.position;
            transform.position = new Vector3(pos.x, y, pos.z);
            yield return null;
        }

        for (float y = -0.5f; y < 0.5f; y += Time.deltaTime / time)
        {
            Vector3 pos = transform.position;
            transform.position = new Vector3(pos.x, y, pos.z);
            yield return null;
        }

    }

    IEnumerator scaleCavern(float start, float target, float time)
    {

        for (float s = start; s < target; s += Time.deltaTime / time)
        {
            transform.localScale = new Vector3(s, s, s);
            yield return null;
        }
    }


}
