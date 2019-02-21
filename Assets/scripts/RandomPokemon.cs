using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using System.Linq;

public class RandomPokemon : MonoBehaviour
{
    [SerializeField]
    private Text pokemonInfo;
    [SerializeField]
    private string pokemonPath;
    [SerializeField]
    private InputField pokemonName;
    [SerializeField]
    private InputField pokemonLevel;
    [SerializeField]
    private Dropdown positiveNatureDropdown;
    [SerializeField]
    private Dropdown negativeNatureDropdown;

    private string[] pokemonFiles;

    // Start is called before the first frame update
    void Start()
    {
        pokemonFiles = Directory.GetFiles(pokemonPath, "*.txt");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GetRandomPokemon(bool bias){
        int rand = UnityEngine.Random.Range(0, pokemonFiles.Length);

        if(bias){
            for(int i = 0; i < pokemonFiles.Length; i++){
                if((pokemonName.text.ToLower()) + ".txt" == Path.GetFileName(pokemonFiles[i])){
                    rand = i;
                    break;
                }
            }
        }

        pokemonInfo.text = GetPokemonText(rand, !bias);
    }

    //Applies leveling up stats upgrades, always respecting the base relations rule.
    //If the nature is set to random or the pokemon is totally random, it's nature is set here.
    public int[] GeneratePokemonStats(int number, int level, ref int posNat, ref int negNat){
        int[] stats = new int[6];
        string allText = System.IO.File.ReadAllText(pokemonFiles[number]);
        string[] fields = allText.Split(';');

        int positiveStats;
        int negativeStats;
        if(posNat == 6)
            positiveStats = UnityEngine.Random.Range(0,6);
        else
            positiveStats = posNat;
        if(negNat == 6)
            negativeStats = UnityEngine.Random.Range(0,6);
        else
            negativeStats = negNat;
        int unspentStats = level + 10;

        stats[0] = Int32.Parse(fields[1]);
        stats[1] = Int32.Parse(fields[2]);
        stats[2] = Int32.Parse(fields[3]);
        stats[3] = Int32.Parse(fields[4]);
        stats[4] = Int32.Parse(fields[5]);
        stats[5] = Int32.Parse(fields[6]);

        if(positiveStats != 0)
            stats[positiveStats] += 2;
        else
            stats[positiveStats] += 1;

        if(negativeStats != 0)
            stats[negativeStats] -= 2;
        else
            stats[negativeStats] -= 1;

        while(unspentStats > 0){
            int rand = UnityEngine.Random.Range(0,6);
            if(rand != positiveStats){
                if(stats[rand] + 1 > stats[positiveStats]){
                    continue;
                }
                if(rand == negativeStats){
                    if(!IsStatSmallest(rand, stats)){
                        continue;
                    }
                }
            }

            stats[rand]++;
            unspentStats--;
        }

        posNat = positiveStats;
        negNat = negativeStats;

        return stats;
    }

    private bool IsStatSmallest(int index, int[] numbers){
        int temp = numbers[index];
        numbers[index]++;
        if(numbers.Min() == temp)
            return true;
        return false;
    }

    //Here the text for the UI text component is generated, as well as handling the
    //pokemon level randomization(if set to true)
    public string GetPokemonText(int number, bool isLevelRandom){
        string formated = null;
        string allText = System.IO.File.ReadAllText(pokemonFiles[number]);
        string[] fields = allText.Split(';');

        int randomLevel;
        if(isLevelRandom || pokemonLevel.text == "")
            randomLevel = UnityEngine.Random.Range(1,101);
        else{
            if(Int32.Parse(pokemonLevel.text) < 1){
                randomLevel = 1;
            }
            else if(Int32.Parse(pokemonLevel.text) > 100){
                randomLevel = 100;
            }
            else{
                randomLevel = Int32.Parse(pokemonLevel.text);
            }
        }

        int posNat;
        int negNat;
        if(isLevelRandom){
            posNat = 6;
            negNat = 6;
        }
        else{
            posNat = positiveNatureDropdown.value;
            negNat = negativeNatureDropdown.value;
        }

        int[] stats = GeneratePokemonStats(number, randomLevel, ref posNat, ref negNat);

        formated += "Name: " + fields[0] + "\n\n";
        formated += "Level: " + randomLevel + "\n";
        formated += "\nHP: " + stats[0];
        if(posNat == 0)
            formated += "(UP)";
        if(negNat == 0)
            formated += "(DOWN)";
        formated += "\nAttack: " + stats[1];
        if(posNat == 1)
            formated += "(UP)";
        if(negNat == 1)
            formated += "(DOWN)";
        formated += "\nDefense: " + stats[2];
        if(posNat == 2)
            formated += "(UP)";
        if(negNat == 2)
            formated += "(DOWN)";
        formated += "\nSpecial Attack: " + stats[3];
        if(posNat == 3)
            formated += "(UP)";
        if(negNat == 3)
            formated += "(DOWN)";
        formated += "\nSpecial Defense: " + stats[4];
        if(posNat == 4)
            formated += "(UP)";
        if(negNat == 4)
            formated += "(DOWN)";
        formated += "\nSpeed: " + stats[5];
        if(posNat == 5)
            formated += "(UP)";
        if(negNat == 5)
            formated += "(DOWN)";
        formated += "\n\nBasic Information:\n";
        formated += "Type: " + fields[7] + "/" + fields[8] + "\n";

        return formated;
    }
}
