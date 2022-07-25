/**
	Index Color Shader for Unity

	Copyright(C) 1997-2021 Web Technology Corp.
	Copyright(C) CRI Middleware Co., Ltd.
	All rights reserved.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEditor;

public static partial class LibraryEditor_IndexColorShader
{
	public static partial class Image
	{
		/* ----------------------------------------------- Variables & Properties */
		#region Variables & Properties
		#endregion Variables & Properties

		/* ----------------------------------------------- Functions */
		#region Functions
		internal static InformationDecode Decode(byte[] dataTextureOriginal, string messageSuffix)
		{
			const string messageLogPrefix = "Decode";

			InformationDecode informationDecode = new InformationDecode();
			if(null == informationDecode)
			{
				LogError(messageLogPrefix, "Not Enough Memory(Information)" + messageSuffix);
				goto Decode_ErrorEnd;
			}

			/* Decode Image-Data */
			informationDecode.Image = Loader.ImageCreate();
			if(null == informationDecode.Image)
			{
				LogError(messageLogPrefix, "Not Enough Memory(Image)" + messageSuffix);
				goto Decode_ErrorEnd;
			}
			IntPtr image = informationDecode.Image;

			informationDecode.HandleDataTexture = GCHandle.Alloc(dataTextureOriginal, GCHandleType.Pinned);
			informationDecode.PointerDataTexture = informationDecode.HandleDataTexture.AddrOfPinnedObject();
			if(0 == Loader.ImageDecode(image, informationDecode.PointerDataTexture, dataTextureOriginal.Length))
			{	/* Failure */
				LogError(messageLogPrefix, "Invalid Image-Data" + messageSuffix);
				goto Decode_ErrorEnd;
			}

			/* Check Image */
			int sizePixelX = Loader.WidthGetImage(image);
			int sizePixelY = Loader.HeightGetImage(image);
			int depthPixel = Loader.DepthGetImage(image);
			int countPalette = Loader.CountGetImageCLUT(image);
			if(false == SizeCheck(sizePixelX, sizePixelY))
			{	/* Invalid */
				LogError(messageLogPrefix, "Invalid Textuer's size" + messageSuffix);
				goto Decode_ErrorEnd;
			}
			if(8 != depthPixel)
			{	/* Invalid */
				LogError(messageLogPrefix, "Invalid Texture's Pixel-Depth" + messageSuffix);
				goto Decode_ErrorEnd;
			}
			if(0 >= countPalette)
			{	/* Invalid */
				LogError(messageLogPrefix, "Texture has no CLUT" + messageSuffix);
				goto Decode_ErrorEnd;
			}

			return(informationDecode);

		Decode_ErrorEnd:;
			informationDecode.Release();

			return(null);
		}
		private static bool SizeCheck(int sizePixelX, int sizePixelY)
		{
			const int sizePixelMinimum = 64;

			if((sizePixelMinimum > sizePixelX) || (sizePixelMinimum > sizePixelY))
			{
				return(false);
			}

			/* MEMO: Should check POT ?? */

			return(true);
		}

		internal static Color[] PaletteConvert(InformationDecode informationDecode, string messageSuffix)
		{
			const string messageLogPrefix = "Convert-Palette";

			/* Generate Palette */
			int sizePalette = Library_IndexColorShader.Data.Palette.CountColorMax;
			Color[] palette = new Color[sizePalette];
			if(null == palette)
			{
				LogError(messageLogPrefix, "Not Enough Memory(Palette)" + messageSuffix);
				return(null);
			}

			/* Clear */
			for(int i=0; i<sizePalette; i++)
			{
				palette[i] = Color.clear;
			}

			/* Decode */
			IntPtr image = informationDecode.Image;
			int countPalette = Loader.CountGetImageCLUT(image);
			for(int i=0; i<countPalette; i++)
			{
				uint colorPalette = Loader.CLUTGetImage(image, i);
				palette[i].a = (float)((colorPalette >> 24) & 0x000000ff) / 255.0f;
				palette[i].r = (float)((colorPalette >> 16) & 0x000000ff) / 255.0f;
				palette[i].g = (float)((colorPalette >> 8) & 0x000000ff) / 255.0f;
				palette[i].b = (float)(colorPalette & 0x000000ff) / 255.0f;
			}

			return(palette);
		}

		internal static Texture2D PixelConvert(InformationDecode informationDecode, string messageSuffix)
		{
			const string messageLogPrefix = "Convert-Pixel";

			/* Create Texture2D */
			IntPtr image = informationDecode.Image;
			int sizePixelX = Loader.WidthGetImage(image);
			int sizePixelY = Loader.HeightGetImage(image);
//			int depthPixel = Loader.DepthGetImage(image);
			Texture2D textureIndexed = new Texture2D(sizePixelX, sizePixelY, TextureFormat.Alpha8, 0, true);	/* Has no mips / ColorSpace is Linear */
			if(null == textureIndexed)
			{
				LogError(messageLogPrefix, "Not Enough Memory(Texture)" + messageSuffix);
				return(null);
			}

			/* Setting */
			textureIndexed.alphaIsTransparency = false;

			/* Decode */
			Color32 colorPixel32 = new Color32();
			for(int y=0; y<sizePixelY; y++)
			{
				for(int x=0; x<sizePixelX; x++)
				{
					uint pixel = Loader.PixelGetImage(image, x, y);
					byte index = (byte)(pixel & 0x000000ff);
					colorPixel32.a = 
					colorPixel32.r = 
					colorPixel32.g = 
					colorPixel32.b = index;

					/* MEMO: Vertical storage direction is reversed between original-image and Unity. */
					textureIndexed.SetPixel(x, (sizePixelY - 1) - y, (Color)colorPixel32);
				}
			}

			return(textureIndexed);
		}
		#endregion Functions

		/* ----------------------------------------------- Enums & Constants */
		#region Enums & Constants
		#endregion Enums & Constants

		/* ----------------------------------------------- Classes, Structs & Interfaces */
		#region Classes, Structs & Interfaces
		internal static partial class Loader
		{
			/* ----------------------------------------------- DLL Imports */
			#region Variables & Properties
//#if UNITY_EDITOR_WIN
//			const string nameDLL_ICSLoaderImage = "ICS_LoaderImage";
//#endif
//#if UNITY_EDITOR_OSX
//			const string nameDLL_ICSLoaderImage = "ICS_LoaderImage";
//#endif
			const string nameDLL_ICSLoaderImage = "ICS_LoaderImage";

			[DllImport(nameDLL_ICSLoaderImage)]
			public static extern IntPtr ImageCreate();

			[DllImport(nameDLL_ICSLoaderImage)]
			public static extern void ImageRelease(IntPtr image);

			[DllImport(nameDLL_ICSLoaderImage)]
			public static extern int ImageDecode(IntPtr image, IntPtr data, int sizeData);

			[DllImport(nameDLL_ICSLoaderImage)]
			public static extern int FormatGetImage(IntPtr image);

			[DllImport(nameDLL_ICSLoaderImage)]
			public static extern int WidthGetImage(IntPtr image);

			[DllImport(nameDLL_ICSLoaderImage)]
			public static extern int HeightGetImage(IntPtr image);

			[DllImport(nameDLL_ICSLoaderImage)]
			public static extern int DepthGetImage(IntPtr image);

			[DllImport(nameDLL_ICSLoaderImage)]
			public static extern int CountGetImageCLUT(IntPtr image);

			[DllImport(nameDLL_ICSLoaderImage)]
			public static extern uint CLUTGetImage(IntPtr image, int index);

			[DllImport(nameDLL_ICSLoaderImage)]
			public static extern uint PixelGetImage(IntPtr image, int positionX, int positionY);

			#endregion Variables & Properties

			/* ----------------------------------------------- Variables & Properties */
			#region Variables & Properties
			#endregion Variables & Properties

			/* ----------------------------------------------- Functions */
			#region Functions
			#endregion Functions

			/* ----------------------------------------------- Enums & Constants */
			#region Enums & Constants
			#endregion Enums & Constants

			/* ----------------------------------------------- Classes, Structs & Interfaces */
			#region Classes, Structs & Interfaces
			#endregion Classes, Structs & Interfaces

			/* ----------------------------------------------- Delegates */
			#region Delegates
			#endregion Delegates
		}

		internal class InformationDecode
		{
			/* ----------------------------------------------- Variables & Properties */
			#region Variables & Properties
			internal GCHandle HandleDataTexture;
			internal IntPtr PointerDataTexture;
			internal IntPtr Image;
			#endregion Variables & Properties

			/* ----------------------------------------------- Functions */
			#region Functions
			internal InformationDecode()
			{
				CleanUp();
			}

			internal void CleanUp()
			{
				PointerDataTexture = IntPtr.Zero;
				Image = IntPtr.Zero;
			}

			internal void Release()
			{
				if(IntPtr.Zero != Image)
				{
					Loader.ImageRelease(Image);
				}

//				PointerDataTexture

				if(true == HandleDataTexture.IsAllocated)
				{
					HandleDataTexture.Free();
				}

				CleanUp();
			}
			#endregion Functions

			/* ----------------------------------------------- Enums & Constants */
			#region Enums & Constants
			#endregion Enums & Constants

			/* ----------------------------------------------- Classes, Structs & Interfaces */
			#region Classes, Structs & Interfaces
			#endregion Classes, Structs & Interfaces

			/* ----------------------------------------------- Delegates */
			#region Delegates
			#endregion Delegates
		}
		#endregion Classes, Structs & Interfaces

		/* ----------------------------------------------- Delegates */
		#region Delegates
		#endregion Delegates
	}
}
