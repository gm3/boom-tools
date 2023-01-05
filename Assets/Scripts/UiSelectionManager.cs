using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiSelectionManager : MonoBehaviour
{

    public GameObject[] clickableSlots;
    public Image[] thisImg;
    public Image[] thisImgThumbnail;
    public Image slotPreviewImg;
    private Color m_MyColor;
    private Color m_MyColor_whenClicked;
    public bool[] isSelected;
    public int currentSlotNumber;
    public Button prevButtonReference;
    public Button nextButtonReference;
   
    // Start is called before the first frame update
    void Start()
    {
        //Button btnPrev = prevButtonReference.GetComponent<Button>();
        //Button btnNext = nextButtonReference.GetComponent<Button>();
        //btnNext.onClick.AddListener(NextSlot(0));   
        //btnPrev.onClick.AddListener(PrevSlot(0)); 

        // set default color values
        m_MyColor = Color.red;
        m_MyColor_whenClicked = Color.blue;
        
        // loop through and get references to all the iamges on the clickable slots
        for (int i = 0; i < clickableSlots.Length; i++)
		{
			thisImg[i] = clickableSlots[i].GetComponent<Image>();
		}

        // initialize
        SelectSlot(0);
 
    }

    // the Select slot method takes in the slot number assigned in the UI event when clicked
    public void SelectSlot(int slotNumber)
    {
        if(slotNumber >= 0 && slotNumber <= clickableSlots.Length)
        {
                
                
                if(!isSelected[slotNumber])
                {
                    for (int i = 0; i < clickableSlots.Length; i++)
                        {
                            thisImg[i].color = m_MyColor; 
                            isSelected[i] = false;
                        }

                        thisImg[slotNumber].color = m_MyColor_whenClicked;
                        isSelected[slotNumber] = true;
                }

                
                slotPreviewImg.sprite = thisImgThumbnail[slotNumber].sprite;
        }

        currentSlotNumber = slotNumber;

    }

    public void PrevSlot(int n)
    {  
            if(currentSlotNumber > 0)
            {
                SelectSlot(currentSlotNumber + n);    
            }
            
    }

    public void NextSlot(int n)
    {
            if(currentSlotNumber < isSelected.Length -1)
            {
                SelectSlot(currentSlotNumber + n);  
            } 
    }
}
