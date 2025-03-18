using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class GuitarTabPlayer : MonoBehaviour
{
    // store dotx y object，Index start at 0
    public GameObject[,] dots = new GameObject[20, 6];
    // Store a string y object where y is a string (1-6)
    public GameObject[] strings = new GameObject[6];
    // The duration of each beat (seconds), which defaults to 0.5 seconds (120 BPM), can be adjusted in the Unity Inspector
    public float beatDuration = 0.5f;

    // Score sequence, based on the fingering of Little Star, one element per beat
    private string[] sequence = new string[]
    {
        // Section 1
        "dot3 5", "dot3 5", "rest", "rest", 
        "dot2 3", "dot2 3", "rest", "rest",  
        "dot3 4", "dot3 4", "dot2 4", "dot2 4", 
        "rest", "rest", "dot3 5", "rest",    
        "rest", "rest", "dot3 4", "dot3 4",    
        "dot2 4", "dot2 4", "rest", "rest",    

        // Section 2
        "rest", "rest", "dot3 4", "dot3 4", 
        "dot2 4", "dot2 4", "rest", "rest",  
        "dot3 5", "dot3 5", "rest", "rest", 
        "dot2 3", "dot2 3", "rest", "rest",  
        "dot3 4", "dot3 4", "dot2 4", "dot2 4",  
        "rest", "rest", "dot3 5", "rest",  

        // Section 3
        "dot1 2", "dot1 2", "dot3 1", "dot3 1", 
        "dot5 1", "dot5 1", "dot3 1", "rest",  
        "dot1 1", "dot1 1", "rest", "rest", 
        "dot3 2", "dot3 2", "dot1 2", "rest",     
        "dot3 1", "dot3 1", "dot1 1", "dot1 1", 
        "rest", "rest", "dot3 2", "rest",   

        // Section 4
        "dot3 1", "dot3 1", "dot1 1", "dot1 1", 
        "rest", "rest", "dot3 2", "rest",  
        "dot1 2", "dot1 2", "dot3 1", "dot3 1", 
        "dot5 1", "dot5 1", "dot3 1", "rest",    
        "dot1 1", "dot1 1", "rest", "rest",     
        "dot3 2", "dot3 2", "dot1 2", "rest"     
    };

    // Color control: The string that needs to be blackened per beat is the same length as the sequence
    private int[] blackStringIndices = new int[]
    {
        // Section 1
        5, 5, 3, 3,
        3, 3, 3, 3,
        4, 4, 4, 4,
        4, 4, 5, 5,
        3, 3, 4, 4,
        4, 4, 4, 4,
        // Section 2
        3, 3, 4, 4,
        4, 4, 4, 4,
        5, 5, 3, 3,
        3, 3, 3, 3,
        4, 4, 4, 4,
        4, 4, 5, 5,
        // Section 3
        2, 2, 1, 1,
        1, 1, 1, 1,
        1, 1, 1, 1,
        2, 2, 2, 2,
        1, 1, 1, 1,
        1, 1, 2, 2,
        // Section 4
        1, 1, 1, 1,
        1, 1, 2, 2,
        2, 2, 1, 1,
        1, 1, 1, 1,
        1, 1, 1, 1,
        2, 2, 2, 2
    };

    void Start()
    {
        // Initialize the dots array to find the dotx y object in the scene
        for (int x = 1; x <= 20; x++)
        {
            for (int y = 1; y <= 6; y++)
            {
                dots[x - 1, y - 1] = GameObject.Find("dot" + x + " " + y);
                if (dots[x - 1, y - 1] == null)
                {
                    Debug.LogWarning("wrong: dot" + x + " " + y);
                }
            }
        }
        // Initializes the strings array
        for (int y = 1; y <= 6; y++)
        {
            strings[y - 1] = GameObject.Find("string" + y);
            if (strings[y - 1] == null)
            {
                Debug.LogWarning("wrong: string" + y);
            }
        }
        // Start the playback coroutine
        StartCoroutine(PlaySequence());
    }

    //Coroutine: Play the music in beat order
    IEnumerator PlaySequence()
    {
        int i = 0; //Add index variable
        foreach (string note in sequence)
        {
            int blackStringIndex = blackStringIndices[i];
            if (note == "rest")
            {
                DeactivateAllDots();
            }
            else
            {
                string[] parts = note.Split(' ');
                int x = int.Parse(parts[0].Substring(3));
                int y = int.Parse(parts[1]);
                ActivateDot(x, y);
            }
            SetStringColor(blackStringIndex, Color.black);
            yield return new WaitForSeconds(beatDuration);
            ResetStringColors(); // Reset the color after each beat
            i++;
        }
        DeactivateAllDots();
    }

    // Close all dotx y objects
    void DeactivateAllDots()
    {
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                if (dots[i, j] != null)
                {
                    dots[i, j].SetActive(false);
                }
            }
        }
    }

    // Active all dotx y objects
    void ActivateDot(int x, int y)
    {
        DeactivateAllDots();
        if (dots[x - 1, y - 1] != null)
        {
            dots[x - 1, y - 1].SetActive(true);
        }
        else
        {
            Debug.LogError("wrong dot: dot" + x + " " + y);
        }
    }

    // Sets the color of the specified string
    void SetStringColor(int stringIndex, Color color)
{
    GameObject stringObj = strings[stringIndex - 1]; // strings 是 GameObject 数组
    if (stringObj != null)
    {
        Image image = stringObj.GetComponent<Image>();
        if (image != null)
        {
            image.color = color; // Reset to default color (white)
        }
        else
        {
            Debug.LogError("string" + stringIndex + " wrong");
        }
    }
    else
    {
        Debug.LogError("string" + stringIndex + " blank");
    }
}

    // Restores color to all strings
    void ResetStringColors()
{
    for (int i = 0; i < strings.Length; i++)
    {
        if (strings[i] != null)
        {
            Image image = strings[i].GetComponent<Image>();
            if (image != null)
            {
                image.color = Color.white; // Reset to default color (white)
            }
        }
    }
}
}