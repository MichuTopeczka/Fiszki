using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FlashcardManager : MonoBehaviour
{
    [Header("Ekrany (Panele)")]
    public GameObject menuPanel; // Panel z przyciskami wyboru jêzyka
    public GameObject gamePanel; // Panel z w³aœciw¹ gr¹ (fiszk¹)

    [Header("Elementy Karty")]
    public TextMeshProUGUI questionText; // Górny tekst (Pytanie)
    public TextMeshProUGUI answerText;   // Dolny tekst (OdpowiedŸ - pocz¹tkowo ukryty)
    public TextMeshProUGUI modeLabel;    // Napis np. "PL -> ENG"
    public Button cardButton;            // Ca³a karta jako przycisk
    public Button nextButton;            // Przycisk "Nastêpne"
    public Button backButton;            // Przycisk powrotu do menu

    // --- Struktura Danych ---
    [System.Serializable]
    public class Word
    {
        public string pl;
        public string en;
        public string de;
    }

    private List<Word> vocabulary = new List<Word>();

    // --- Zmienne stanu ---
    private int currentIndex = 0;
    private bool isAnswerVisible = false;

    // Tryby: 0=PL->ENG, 1=ENG->PL, 2=DE->PL, 3=PL->DE
    private int currentMode = 0;

    void Start()
    {
        LoadVocabulary();

        // Na starcie pokazujemy Menu, a ukrywamy Grê
        menuPanel.SetActive(true);
        gamePanel.SetActive(false);

        // Przypisanie funkcji do przycisków
        cardButton.onClick.AddListener(RevealAnswer);
        nextButton.onClick.AddListener(NextCard);
        backButton.onClick.AddListener(BackToMenu);
    }

    void LoadVocabulary()
    {
        vocabulary.Add(new Word { pl = "Kot", en = "Cat", de = "Katze" });
        vocabulary.Add(new Word { pl = "Pies", en = "Dog", de = "Hund" });
        vocabulary.Add(new Word { pl = "Samochód", en = "Car", de = "Auto" });
        vocabulary.Add(new Word { pl = "Dom", en = "House", de = "Haus" });
        vocabulary.Add(new Word { pl = "Dziêkujê", en = "Thank you", de = "Danke" });
        vocabulary.Add(new Word { pl = "Woda", en = "Water", de = "Wasser" });
        vocabulary.Add(new Word { pl = "Jab³ko", en = "Apple", de = "Apfel" });
    }

    // --- Funkcja wywo³ywana przez przyciski w MENU ---
    public void StartGame(int modeIndex)
    {
        currentMode = modeIndex;
        menuPanel.SetActive(false); // Ukryj menu
        gamePanel.SetActive(true);  // Poka¿ grê
        NextCard(); // Wylosuj pierwsz¹ kartê
    }

    public void BackToMenu()
    {
        gamePanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    void ShowCard()
    {
        Word w = vocabulary[currentIndex];
        isAnswerVisible = false; // Resetujemy widocznoœæ odpowiedzi

        // Ustawienie tekstów w zale¿noœci od trybu
        switch (currentMode)
        {
            case 0: // PL -> ENG
                modeLabel.text = "Polski -> Angielski";
                questionText.text = w.pl;
                answerText.text = w.en;
                break;
            case 1: // ENG -> PL
                modeLabel.text = "Angielski -> Polski";
                questionText.text = w.en;
                answerText.text = w.pl;
                break;
            case 2: // DE -> PL
                modeLabel.text = "Niemiecki -> Polski";
                questionText.text = w.de;
                answerText.text = w.pl;
                break;
            case 3: // PL -> DE
                modeLabel.text = "Polski -> Niemiecki";
                questionText.text = w.pl;
                answerText.text = w.de;
                break;
        }

        // Na pocz¹tku ukrywamy odpowiedŸ (zmieniamy kolor na przezroczysty lub wy³¹czamy obiekt)
        answerText.gameObject.SetActive(false);
    }

    public void RevealAnswer()
    {
        // Jeœli odpowiedŸ jest ukryta, poka¿ j¹
        if (!isAnswerVisible)
        {
            answerText.gameObject.SetActive(true);
            isAnswerVisible = true;
        }
    }

    public void NextCard()
    {
        // Losowanie
        int newIndex = Random.Range(0, vocabulary.Count);
        while (newIndex == currentIndex && vocabulary.Count > 1)
        {
            newIndex = Random.Range(0, vocabulary.Count);
        }
        currentIndex = newIndex;
        ShowCard();
    }
}