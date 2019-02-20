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

    public void GetRandomPokemon(){
        int rand = UnityEngine.Random.Range(0, pokemonFiles.Length);

        pokemonInfo.text = GetPokemonText(rand);
    }

    public int[] GeneratePokemonStats(int number, int level){
        int[] stats = new int[6];
        string allText = System.IO.File.ReadAllText(pokemonFiles[number]);
        string[] fields = allText.Split(';');

        int positiveStats = UnityEngine.Random.Range(0,6);
        int negativeStats = UnityEngine.Random.Range(0,6);
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

        return stats;
    }

    private bool IsStatSmallest(int index, int[] numbers){
        int temp = numbers[index];
        numbers[index]++;
        if(numbers.Min() == temp)
            return true;
        return false;
    }

    public string GetPokemonText(int number){
        string formated = null;
        string allText = System.IO.File.ReadAllText(pokemonFiles[number]);
        string[] fields = allText.Split(';');

        int randomLevel = UnityEngine.Random.Range(1,101);
        int[] stats = GeneratePokemonStats(number, randomLevel);

        formated += "Name: " + fields[0] + "\n\n";
        formated += "Level: " + randomLevel + "\n";
        formated += "\nHP: " + stats[0];
        formated += "\nAttack: " + stats[1];
        formated += "\nDefense: " + stats[2];
        formated += "\nSpecial Attack: " + stats[3];
        formated += "\nSpecial Defense: " + stats[4];
        formated += "\nSpeed: " + stats[5];
        formated += "\n\nBasic Information:\n";
        formated += "Type: " + fields[7] + "/" + fields[8] + "\n";

        return formated;
    }
}
