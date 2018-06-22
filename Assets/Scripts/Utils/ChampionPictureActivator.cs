using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChampionPictureActivator {

    // activates the champion picture in the given container

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

    public static void ActivateChampionPictureB(GameObject pictureContainerParent, string pictureContainerName)
    {
        // find picture container of the correct champion class (
        foreach (Transform pictureContainer in pictureContainerParent.transform)
        {
            // find and activate the picture container
            if (pictureContainer.gameObject.name == pictureContainerName)
            {
                pictureContainer.gameObject.SetActive(true);
            }

            // disable all other picture containers
            else
            {
                // ignore any buttons, texts that are attached to the object
                if (pictureContainer.gameObject.GetComponent<Button>() == null && pictureContainer.gameObject.GetComponent<Text>() == null)
                    pictureContainer.gameObject.SetActive(false);
            }
        }
    }
}
