﻿//———————————————————————–
// <copyright file=”Resources.cs” game="KzzzZZZzzT!">
//     Copyright (c) Extreme Z7.  All rights reserved.
// </copyright>
//———————————————————————–
using UnityEngine;
using System.Collections.Generic;

namespace Helper
{
    public static class Res : System.Object
    {
        //fields
        const string texturesFolder = "Textures/";
        const string itemSpritesFolder = texturesFolder + "ItemSprites/";
        const string characterProfilesFolder = texturesFolder + "CharacterProfiles/";
        const string saveThumbnailsFolder = texturesFolder + "SaveFileThumbnails/";
        const string otherTexturesFolder = texturesFolder + "OtherTextures/";

        //properties
        public static Object KrazyKrystalSprite{ get; private set; }

        public static Object GemSilhouette{ get; private set; }

        public static Object RedGemSprite{ get; private set; }

        public static Object GreenGemSprite{ get; private set; }

        public static Object OrangeGemSprite{ get; private set; }

        public static Object BlueGemSprite{ get; private set; }

        public static Object PurpleGemSprite{ get; private set; }

        public static Object DiamondGemSprite{ get; private set; }

        public static Object LetterKSprite{ get; private set; }

        public static Object LetterKSilhouette{ get; private set; }

        public static Object LetterZSprite{ get; private set; }

        public static Object LetterZSilhouette{ get; private set; }

        public static Object LetterTSprite{ get; private set; }

        public static Object LetterTSilhouette{ get; private set; }

        public static List<Object> SaveThumbnails{ get; private set; }

        public static Object SaveFileSelected{ get; private set; }

        public static Object SaveFileNotSelected{ get; private set; }

        public static List<Object> CharacterProfiles{ get; private set; }

        //constructor
        static Res()
        {
            // Load all the texture resources
            //
            KrazyKrystalSprite = Resources.Load(itemSpritesFolder
                + "KrazyKrystal", typeof(Sprite));
            GemSilhouette = Resources.Load(itemSpritesFolder
                + "GemSilhuoette");
            RedGemSprite = Resources.Load(itemSpritesFolder
                + "RidiculousRuby", typeof(Sprite));
            GreenGemSprite = Resources.Load(itemSpritesFolder
                + "IffyEmerald", typeof(Sprite));
            OrangeGemSprite = Resources.Load(itemSpritesFolder
                + "TrickyTopaz", typeof(Sprite));
            BlueGemSprite = Resources.Load(itemSpritesFolder
                + "SillySapphire", typeof(Sprite));
            PurpleGemSprite = Resources.Load(itemSpritesFolder
                + "AbsurdAmethyst", typeof(Sprite));
            DiamondGemSprite = Resources.Load(itemSpritesFolder
                + "DiamondBolt", typeof(Sprite));
            LetterKSprite = Resources.Load(itemSpritesFolder
                + "LetterK", typeof(Sprite));
            LetterKSilhouette = Resources.Load(itemSpritesFolder
                + "LetterKSilhuoette");
            LetterZSprite = Resources.Load(itemSpritesFolder
                + "LetterZ", typeof(Sprite));
            LetterZSilhouette = Resources.Load(itemSpritesFolder
                + "LetterZSilhuoette");
            LetterTSprite = Resources.Load(itemSpritesFolder
                + "LetterT", typeof(Sprite));
            LetterTSilhouette = Resources.Load(itemSpritesFolder
                + "LetterTSilhuoette");

            SaveThumbnails = new List<Object>();

            //Loads the first save file thumbnail
            SaveThumbnails.Add(Resources.Load(saveThumbnailsFolder
                    + "NoSaveThumb"));

            //Loads every other save file thumbnail
            //
            for (int i = 0; i < 25; i++)
            {
                Object saveThumb = Resources.Load(saveThumbnailsFolder
                                       + "LevelThumb" + i.ToString("D3"));

                if (saveThumb != null)
                {
                    SaveThumbnails.Add(saveThumb);
                }
                else
                {
                    continue;
                }
            }

            SaveFileSelected = Resources.Load(otherTexturesFolder
                + "SaveFileSelected");
            SaveFileNotSelected = Resources.Load(otherTexturesFolder
                + "SaveFileNotSelected");

            CharacterProfiles = new List<Object>();

            CharacterProfiles.Add(Resources.Load(characterProfilesFolder
                    + "KZTHead", typeof(Sprite)));
            CharacterProfiles.Add(Resources.Load(characterProfilesFolder
                    + "KrankyHead", typeof(Sprite)));
            CharacterProfiles.Add(Resources.Load(characterProfilesFolder
                    + "Kommodore64Head", typeof(Sprite)));
            CharacterProfiles.Add(Resources.Load(characterProfilesFolder
                    + "KrushBandicootHead", typeof(Sprite)));
            CharacterProfiles.Add(Resources.Load(characterProfilesFolder
                    + "DrWackoHead", typeof(Sprite)));
            CharacterProfiles.Add(Resources.Load(characterProfilesFolder
                    + "DrKrazyHead", typeof(Sprite)));
            CharacterProfiles.Add(null);
        }

        //methods
        /// <summary>
        /// Either returns the red gem sprite image or its silhuoette
        /// </summary>
        /// <returns>The red gem texture.</returns>
        /// <param name="notASilhuoette">If set to <c>true</c> then not a silhuoette.</param>
        public static Object GetRedGemSprite(bool notASilhuoette = true)
        {
            return notASilhuoette ? RedGemSprite : GemSilhouette;
        }

        /// <summary>
        /// Either returns the green gem sprite image or its silhuoette
        /// </summary>
        /// <returns>The green gem sprite.</returns>
        /// <param name="notASilohuette">If set to <c>true</c> then not a silohuette.</param>
        public static Object GetGreenGemSprite(bool notASilohuette = true)
        {
            return notASilohuette ? GreenGemSprite : GemSilhouette;
        }

        public static Object GetOrangeGemSprite()
        {
            return OrangeGemSprite;
        }

        public static Object GetBlueGemSprite()
        {
            return BlueGemSprite;
        }

        public static Object GetPurpleGemSprite()
        {
            return PurpleGemSprite;
        }

        public static Object GetDiamondGemSprite()
        {
            return DiamondGemSprite;
        }

        /// <summary>
        /// Either returns the letter K sprite image or its silhuoette
        /// </summary>
        /// <returns>The letter K sprite.</returns>
        /// <param name="notASilohuette">If set to <c>true</c> then not a silohuette.</param>
        public static Object GetLetterKSprite(bool notASilohuette = true)
        {
            return notASilohuette ? LetterKSprite : LetterKSilhouette;
        }


        /// <summary>
        /// Either returns the letter Z sprite image or its silhuoette
        /// </summary>
        /// <returns>The letter Z sprite.</returns>
        /// <param name="notASilohuette">If set to <c>true</c> then not a silohuette.</param>
        public static Object GetLetterZSprite(bool notASilohuette = true)
        {
            return notASilohuette ? LetterZSprite : LetterZSilhouette;
        }

        /// <summary>
        /// Either returns the letter T sprite image or its silhuoette
        /// </summary>
        /// <returns>The letter T sprite.</returns>
        /// <param name="notASilohuette">If set to <c>true</c> then not a silohuette.</param>
        public static Object GetLetterTSprite(bool notASilohuette = true)
        {
            return notASilohuette ? LetterTSprite : LetterTSilhouette;
        }

        /// <summary>
        /// Gets the no save thumbnail.
        /// </summary>
        /// <returns>The no save thumbnail.</returns>
        public static Object GetNoSaveThumbnail()
        {
            return SaveThumbnails[0];
        }

        /// <summary>
        /// Gets the save thumbnail.
        /// </summary>
        /// <returns>The save thumbnail.</returns>
        /// <param name="worldIndex">World index.</param>
        /// <param name="levelIndex">Level index.</param>
        public static Object GetSaveThumbnail(int worldIndex, int levelIndex)
        {
            if (worldIndex < 0 || levelIndex < 0)
            {
                throw new System.ArgumentException("World index or level index"
                    + " cannot be less than zero.");
            }
            else if (worldIndex > 4 || levelIndex > 4)
            {
                throw new System.ArgumentException("World index or level index"
                    + " cannot be larger than four.");
            }

            int totalIndex = worldIndex * 5 + levelIndex + 1;
            return SaveThumbnails[totalIndex];
        }

        /// <summary>
        /// Gets the save file texture.
        /// </summary>
        /// <returns>The save file texture.</returns>
        /// <param name="selected">If set to <c>true</c> selected.</param>
        public static Object GetSaveFileTexture(bool selected)
        {
            return selected ? SaveFileSelected : SaveFileNotSelected;
        }

        /// <summary>
        /// Gets the character profile.
        /// </summary>
        /// <returns>The character profile.</returns>
        /// <param name="index">Index.</param>
        public static Object GetCharacterProfile(int index)
        {
            return CharacterProfiles[index];
        }

        /// <summary>
        /// Gets the inventory item sprite.
        /// </summary>
        /// <returns>The inventory item sprite.</returns>
        /// <param name="itemIndex">Item index.</param>
        /// <param name="levelIndex">Level index.</param>
        /// <param name="worldIndex">World index.</param>
        public static Object GetInventoryItemSprite(int itemIndex, int levelIndex, int worldIndex)
        {
            //This is a temporary function that will be removed in due time
            switch (levelIndex)
            {
                case 1:
                case 2:
                case 3:
                    switch (itemIndex)
                    {
                        case 0:
                            return GetRedGemSprite();
                        case 1:
                            return GetLetterKSprite();
                        case 2:
                            return GetLetterZSprite();
                        case 3:
                            return GetLetterTSprite();
                        default:
                            return null;
                    }
                case 4:
                    if (itemIndex == 0)
                    {
                        switch (worldIndex)
                        {
                            case 1:
                                return GetGreenGemSprite();
                            case 2:
                                return GetOrangeGemSprite();
                            case 3:
                                return GetBlueGemSprite();
                            case 4:
                                return GetPurpleGemSprite();
                            case 5:
                                return GetDiamondGemSprite();
                            default:
                                return null;
                        }
                    }
                    else
                        return null;
                default:
                    return null;
            }
        }
    }
}
