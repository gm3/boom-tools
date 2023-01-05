using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProgressBar : MonoBehaviour
{
        public DNAManager dnaManagerReference;
        public Slider progressSlider;
        public RandomRoll randomRollRef;
        public TextMeshProUGUI mText;
        public GameObject mTextGaameObjectRef;

    // Start is called before the first frame update
    void Start()
    {
        mTextGaameObjectRef.SetActive(false);
        progressSlider.maxValue = randomRollRef.totalSpins;
    }

    // Update is called once per frame
    void Update()
    {
        progressSlider.value = dnaManagerReference.genID;

        if(progressSlider.value == progressSlider.maxValue)
        {
            mTextGaameObjectRef.SetActive(true);
        }
    }
}
