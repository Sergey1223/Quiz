using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class QuestionGenerator : MonoBehaviour
{
    [SerializeField]
    [HideInInspector]
    private List<string> _previewAnswers = new List<string>();

    [SerializeField]
    [HideInInspector]
    private string _currentAnswer;

    [SerializeField]
    private Text _label;

    [SerializeField]
    [Tooltip("Text pattern for generation question. Use '{0}' to specify place for card data name. Example: Find '{0}'")]
    private string _textPattern;

    [SerializeField]
    private AudioClip _rigthAnswerReaction;

    [SerializeField]
    private AudioClip _wrongAnswerReaction;

    public Text Label => _label;

    public string TextPattern => _textPattern;

    public AudioClip RigthAnswerReaction => _rigthAnswerReaction;

    public AudioClip WrongAnswerReaction => _wrongAnswerReaction;

    private List<CardData> FindVariants(CardBundleData bundleData)
    {
        List<CardData> variants = new List<CardData>(bundleData.Cards);

        if (_previewAnswers.Count != 0)
        {
            foreach (string answer in _previewAnswers)
            {
                CardData variant = variants.Find((v) => answer.Equals(v.Identifier));

                if (variant != null)
                {
                    variants.Remove(variant);
                }
            }
        }

        if (variants.Count == 0)
        {
            foreach (CardData cardData in bundleData.Cards)
            {
                _previewAnswers.Remove(cardData.Identifier);
            }

            return FindVariants(bundleData);
        }

        return variants;
    }

    private void ResetLabel()
    {
        Label.color = new Color(Label.color.r, Label.color.g, Label.color.b, 0f);

        Label.DOFade(1f, 1f);
    }

    private void PlayReactionSound(bool checkResult)
    {
        if (TryGetComponent(out AudioSource audioSource))
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            audioSource.clip = checkResult ? _rigthAnswerReaction : _wrongAnswerReaction;

            audioSource.Play();
        }
    }

    public void Generate(CardBundleData bundleData, bool resetLabel)
    {
        List<CardData> variants = FindVariants(bundleData);

        int index = UnityEngine.Random.Range(0, variants.Count);

        _currentAnswer = variants[index].Identifier;
        _previewAnswers.Add(_currentAnswer);

        _label.text = string.Format(_textPattern, variants[index].Name);

        if (resetLabel)
        {
            ResetLabel();
        }
    }

    public bool CheckAnswer(CardData data)
    {
        bool result = _currentAnswer.Equals(data.Identifier);

        PlayReactionSound(result);

        return result;
    }
}
