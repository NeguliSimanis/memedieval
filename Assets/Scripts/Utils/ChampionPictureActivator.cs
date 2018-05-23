using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChampionPictureActivator {

    // activates a default champion picture in the given container

    public static void ActivateChampionPicture(GameObject pictureContainer, string pictureName)
    {
        foreach (Transform picture in pictureContainer.transform)
        {
            // find and activate the picture
            if (picture.gameObject.name == pictureName)
            {
                picture.gameObject.SetActive(true);
            }

            // disable all other pictures in the container
            else
            {
                // ignore any buttons, texts that are attached to the object
                if (picture.gameObject.GetComponent<Button>() == null && picture.gameObject.GetComponent<Text>() == null)
                    picture.gameObject.SetActive(false);
            }
        }
    }
}
