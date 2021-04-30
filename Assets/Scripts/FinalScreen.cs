using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalScreen : MonoBehaviour
{
    public void ButtonPressed()
    {
        SceneManager.LoadScene("Main menu");
    }
}
