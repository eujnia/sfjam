using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct SubtitleOnRoad
{
    public string text;
    public Vector2 activeRange;
    public Color color;
}

[System.Serializable]
public struct SubtitleOnBlackScreen
{
    public string text;
    public int sequenceNumber;
    public Color color;
}


public class Subtitles : MonoBehaviour
{
    public SubtitleOnRoad[] subtitles1;
    public SubtitleOnBlackScreen[] subtitles2;
    public SubtitleOnRoad[] subtitles3;
    public SubtitleOnBlackScreen[] subtitles4;
    public SubtitleOnRoad[] subtitles5;
    public SubtitleOnBlackScreen[] subtitles6;
    public SubtitleOnRoad[] subtitles7;
    public SubtitleOnBlackScreen[] final;
    public SubtitleOnRoad[] xd;

    Color targetColor = Color.white;
    public int imageOption = 0;
    int currentSequenceNumber = 0;

    TMPro.TextMeshProUGUI textMesh;
    Image image;
    GameManager gameManager;
    BlackTransitionEffect blackTransitionEffect;
    MoveAlongSpline moveAlongSpline;
    GameObject skipButton;
    float alpha;
    float imageInitialAlpha;
    float timer = 0f;

    void Start()
    {
        textMesh = transform.Find("Text").GetComponent<TMPro.TextMeshProUGUI>();
        image = transform.Find("Background").GetComponent<Image>();
        imageInitialAlpha = image.color.a;
        moveAlongSpline = GameObject.Find("Player").GetComponent<MoveAlongSpline>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        blackTransitionEffect = GameObject.Find("BlackTransition").GetComponent<BlackTransitionEffect>();
        skipButton = GameObject.Find("SkipButton").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.level == 1 || gameManager.level == 3 || gameManager.level == 5 || gameManager.level == 7 || gameManager.level == 11)
            ManageRoadSubtitles();
        else
            ManageBlackScreenSubtitles();

        ManageSequence();
        skipButton.SetActive(imageOption != 0);
        
    }

    private void ManageSequence()
    {
        if (currentSequenceNumber == 1)
        {
            timer += Time.deltaTime;
            if (timer > 20f)
            {
                currentSequenceNumber = 0;
                imageOption = 0;
                timer = 0f;
                gameManager.AdvanceLevel();
                GameObject.Find("ImageOptions").GetComponent<ImageOptions>().UpdateSprites();
                blackTransitionEffect.GoTransparent(() => { });
            }
        }
    }

    private void ManageBlackScreenSubtitles()
    {
        if (gameManager.level == 8 || gameManager.level == 9) return;

        SubtitleOnBlackScreen[] subtitlesToUse = subtitles2;
        if (gameManager.level == 4) subtitlesToUse = subtitles4;
        if (gameManager.level == 6) subtitlesToUse = subtitles6;
        if (gameManager.level == 10) subtitlesToUse = final;

        foreach (SubtitleOnBlackScreen subtitle in subtitlesToUse)
        {
            if (currentSequenceNumber == subtitle.sequenceNumber)
            {
                textMesh.text = ReplaceTags(subtitle.text);
                targetColor = subtitle.color;
                break;
            }
        }

        textMesh.color = Color.Lerp(textMesh.color, targetColor, Time.deltaTime * 70f);

        textMesh.alpha = 1;
        image.color = new Color(image.color.r, image.color.g, image.color.b, imageInitialAlpha * 1);
    }

    private void ManageRoadSubtitles()
    {
        float t = moveAlongSpline.distancePercentage;
        // float t = gameManager.completionPercentage;
        bool active = false;

        SubtitleOnRoad[] subtitlesToUse = subtitles1;
        if (gameManager.level == 3) subtitlesToUse = subtitles3;
        if (gameManager.level == 5) subtitlesToUse = subtitles5;
        if (gameManager.level == 7) subtitlesToUse = subtitles7;
        if (gameManager.level == 11) subtitlesToUse = xd;

        foreach (SubtitleOnRoad subtitle in subtitlesToUse)
        {
            if (t >= subtitle.activeRange.x && t <= (subtitle.activeRange.y == 0 ? subtitle.activeRange.x + 0.03f : subtitle.activeRange.y))
            {
                textMesh.text = ReplaceTags(subtitle.text);
                targetColor = subtitle.color;

                active = true;
                break;
            }
        }

        if (active)
        {
            alpha += Time.deltaTime * 2f;
        }
        else
        {
            alpha -= Time.deltaTime * 2f;
        }

        textMesh.color = Color.Lerp(textMesh.color, targetColor, Time.deltaTime * 70f);

        alpha = Mathf.Clamp01(alpha);
        textMesh.alpha = alpha;
        image.color = new Color(image.color.r, image.color.g, image.color.b, imageInitialAlpha * alpha);

    }

    private string ReplaceTags(string text)
    {
        switch (gameManager.level)
        {
            case 2:
                if (imageOption == 1) text = text.Replace("{2}", "* {0} y {1} terminaron convencidos de que el vino blanco es el mejor vino *");
                else if (imageOption == 2) text = text.Replace("{2}", "* No lo tomaron, el vino quedó en la cocina, la mamá de {1} lo vió, el pesca apareció en la historia *");
                else if (imageOption == 3) text = text.Replace("{2}", "* Ni {0} ni {1} entienden como hay gente que disfrute de un sabor tan pero tan feo *");
                break;
            case 4:
                if (imageOption == 1) text = text.Replace("{2}", "* {1} te cocinó, matarías por volver a saborear algo así *");
                else if (imageOption == 2) text = text.Replace("{2}", "* A veces hace falta que alguien te haga probar cosas nuevas, querés mucho a {1} *");
                else if (imageOption == 3) text = text.Replace("{2}", "* {1} enamoró a {0} *");
                break;
            case 6:
                if (imageOption == 1) text = text.Replace("{2}", "* {1} espera que fumar porro no sea el mejor plan que a {0} se le pueda ocurrir *");
                else if (imageOption == 2) text = text.Replace("{2}", "* {0} hizo que {1} empiece a fumar a diario, hay que ser hijo de puta... *");
                else if (imageOption == 3) text = text.Replace("{2}", "* {0} sabe que su parlante va a sonar mejor que cualquier verga que {1} tenga en su casa. {1} se quiere muy poco *");
                break;
            case 10:
                if (imageOption == 2) text = text.Replace("{2}", "Y parece que eso nunca va a cambiar");
                break;
        }


        text = text.Replace("{0}", char.ToUpper(Config.Instance.data.myName[0]) + Config.Instance.data.myName.Substring(1).ToLower());
        text = text.Replace("{1}", char.ToUpper(Config.Instance.data.otherName[0]) + Config.Instance.data.otherName.Substring(1).ToLower());

        return text;
    }

    public void SelectImageOption(int option)
    {
        if (imageOption != 0)
        {
            timer += 5f;
            return;
        }
        imageOption = option;
        currentSequenceNumber++;
    }

    public void FinishTimer()
    {
        timer += 20f;
    }


    public static float Similarity(string s1, string s2)
    {
        if (string.IsNullOrEmpty(s1) || string.IsNullOrEmpty(s2))
            return 0;

        s1 = s1.Trim().ToLower();
        s2 = s2.Trim().ToLower();

        int distance = LevenshteinDistance(s1, s2);
        int maxLen = Math.Max(s1.Length, s2.Length);

        if (maxLen == 0) return 1.0f;

        // Convertimos la distancia en similitud
        return 1.0f - (float)distance / maxLen;
    }

    private static int LevenshteinDistance(string s, string t)
    {
        int n = s.Length;
        int m = t.Length;
        int[,] d = new int[n + 1, m + 1];

        // Inicialización
        for (int i = 0; i <= n; i++) d[i, 0] = i;
        for (int j = 0; j <= m; j++) d[0, j] = j;

        // DP
        for (int i = 1; i <= n; i++)
        {
            for (int j = 1; j <= m; j++)
            {
                int cost = (s[i - 1] == t[j - 1]) ? 0 : 1;

                d[i, j] = Math.Min(
                    Math.Min(
                        d[i - 1, j] + 1,      // eliminación
                        d[i, j - 1] + 1),     // inserción
                    d[i - 1, j - 1] + cost  // sustitución
                );
            }
        }

        return d[n, m];
    }
}


