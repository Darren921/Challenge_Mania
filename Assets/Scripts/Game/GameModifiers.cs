using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameModifiers : MonoBehaviour
{
    public static float stageNumber = 1;
    public static float score;
    public static float playerHealthModifer;
    public static float playerDamageModifer;   
    public static float enemyHealthModifer;
    public static float enemyDamageModifer;
    public static int enemyCountModifer;
    public static float overallScoreModifer;

    
    [SerializeField] TextMeshProUGUI stageNumberText;
    
    [SerializeField] TextMeshProUGUI[] ChallengeDisplays;
    [SerializeField] private TextMeshProUGUI[] ChallengeTypeDisplays;
    [SerializeField] private Image[] ChallengeModeDisplays;
    [SerializeField] private Sprite survivalLogo; 
    [SerializeField] private Sprite killLogo;


    
    private void Awake()
    {
        GenerateModifiers();
    }

    [Serializable]
    private struct ModiferSet
    {
       public GameManager.GameMode gameMode;
        public float LPlayerHealthModifer;
        public float LPlayerDamageModifer;
        public float LEnemyHealthModifer;
        public float LEnemyDamageModifer;
        public int LEnemyCountModifer;
        public float overallScoreModifer;
    }


    private List<ModiferSet> _modiferSets = new();

    private void GenerateModifiers()
    {

        _modiferSets.Clear();

        for (var i = 0; i < 3; i++)
        {
           var newModifierSet =new ModiferSet
           {
               gameMode = (GameManager.GameMode)Mathf.RoundToInt(Random.Range(1f, 2f)),
               LPlayerHealthModifer = Mathf.Clamp((float)Math.Round(Random.Range(-stageNumber * 1.25f, stageNumber * 1.25f), 2), -0.50f,
                       0.50f),
               LPlayerDamageModifer = Mathf.Clamp((float)Math.Round(Random.Range(-stageNumber * 1.25f, stageNumber * 1.25f), 2), -0.50f,
                       0.50f),
               LEnemyHealthModifer = Mathf.Clamp((float)Math.Round(Random.Range(-stageNumber * 1.25f, stageNumber * 1.25f), 2), -0.50f,
                       0.50f),
               LEnemyDamageModifer = Mathf.Clamp((float)Math.Round(Random.Range(-stageNumber * 1.25f, stageNumber * 1.25f), 2), -0.50f,
                       0.50f),
               LEnemyCountModifer = (int)Mathf.Clamp(Mathf.RoundToInt(Random.Range(-stageNumber * 5f, stageNumber * 5f)), -9f, 10f),
           
           };
           var overallModifier = newModifierSet.LPlayerDamageModifer + newModifierSet.LPlayerHealthModifer - (newModifierSet.LEnemyCountModifer * 0.5f +  newModifierSet.LEnemyDamageModifer + newModifierSet.LEnemyHealthModifer);
           print(overallModifier);
           newModifierSet.overallScoreModifer = overallModifier switch
           {
               < -1.5f => stageNumber * 1.25f,
               >= -1.5f and < 1.39f => stageNumber * 1, 
               >= 1.4f and <=2 => stageNumber * 0.8f,
               >= 2 => stageNumber * 0.5f,
               _ => newModifierSet.overallScoreModifer
           };
           

           _modiferSets.Add(newModifierSet);
        }
        DisplayModifiers();
    } 

    private void DisplayModifiers()
    {
        for (var i = 0; i < _modiferSets.Count; i++)
        {
            var set = _modiferSets[i];
            var gameMode = set.gameMode;
            var pHealth = set.LPlayerHealthModifer.ToString("+0%;-0%");
            var pDamage = set.LPlayerDamageModifer.ToString("+0%;-0%");
            var eHealth = set.LEnemyHealthModifer.ToString("+0%;-0%");
            var eDamage = set.LEnemyDamageModifer.ToString("+0%;-0%");
            var eCount  = set.LEnemyCountModifer.ToString("+0;-0");
            var ScoreMod = set.overallScoreModifer.ToString();
            ChallengeModeDisplays[i].sprite = gameMode == GameManager.GameMode.Kill ? killLogo : survivalLogo;
            ChallengeTypeDisplays[i].text = gameMode.ToString();
            ChallengeDisplays[i].text = $"Player:  \\n  HP {pHealth}  \\n  DMG {pDamage} \n  Enemy:  \\n  HP {eHealth} \n DMG {eDamage} \\n Count {eCount} \\n Score Multiplier {ScoreMod}";
        }

        stageNumberText.text = $"Stage {stageNumber.ToString()}" ;
    }

    public void SelectModifer(int value)
    {
        var modiferChosen = _modiferSets[value];
        GameManager.CurGameMode = modiferChosen.gameMode;
        playerHealthModifer = modiferChosen.LPlayerHealthModifer;
        playerDamageModifer = modiferChosen.LPlayerDamageModifer;
        enemyCountModifer = modiferChosen.LEnemyCountModifer;
        enemyDamageModifer = modiferChosen.LEnemyDamageModifer;
        enemyHealthModifer = modiferChosen.LEnemyHealthModifer;
        overallScoreModifer = modiferChosen.overallScoreModifer;
        SceneManager.LoadScene("Main");
    }
}
