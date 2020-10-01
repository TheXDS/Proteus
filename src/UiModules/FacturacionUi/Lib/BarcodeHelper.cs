/*
Copyright © 2017-2020 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using BarcodeLib;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TheXDS.MCART;
using TheXDS.Proteus.Models;
using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.FacturacionUi.Lib
{
    internal static class BarcodeHelper
    {
        public static Visual RenderBarcode(TYPE barcodeType, string id)
        {
            var v = new DrawingVisual();
            using var dc = v.RenderOpen();
            dc.DrawImage(GetBarcode(barcodeType, id), new Rect
            { 
                Width = 200,
                Height = 100
            });
            return v;
        }

        public static BitmapSource GetBarcode(TYPE barcodeType, string id)
        {
            try
            {
                var v = new DrawingVisual();
                using var dc = v.RenderOpen();
                using var barcode = new Barcode
                {
                    IncludeLabel = true,
                    LabelPosition = LabelPositions.BOTTOMCENTER,
                    ImageFormat = System.Drawing.Imaging.ImageFormat.Png,
                };
                return barcode.Encode(barcodeType, id).ToSource();
            }
            catch
            {
                if (barcodeType != TYPE.CODE128B)
                {
                    return GetBarcode(TYPE.CODE128B, id);
                }
                throw;
            }
        }

        public static Visual GenerateBarcodes(TYPE barcodeType, params Facturable[] items)
        {
            return GenerateBarcodes(barcodeType, items.AsEnumerable());
        }

        public static Visual GenerateBarcodes(TYPE barcodeType, IEnumerable<Facturable> items)
        {
            var roth = new UniformGrid
            {
                Columns = 2
            };
            foreach (var j in items)
            {
                roth.Children.Add(GenerateBarcodeBlock(barcodeType,j));
            }
            return roth;
        }

        public static UIElement GenerateBarcodeBlock(TYPE barcodeType, Facturable j)
        {
            return new Border
            {
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1),
                Child =
                new DockPanel
                {
                    Children =
                    {
                        new Image
                        {
                            Source = GetBarcode(barcodeType, j.StringId),
                            MaxWidth = 200, MaxHeight = 150,
                            //Margin = new Thickness(20, 0, 10, 10)
                        },
                        new StackPanel
                        {
                            Children =
                            {
                                new TextBlock { Text = j.Name },
                                new Separator(),
                                new TextBlock { Text = GetDetails(j) }
                            },
                            VerticalAlignment = VerticalAlignment.Top,
                            HorizontalAlignment = HorizontalAlignment.Left
                        }
                    },
                    //Margin = new Thickness(0, 20, 20, 0),
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left,
                },
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
            };
        }


        private static string? GetDetails(Facturable i)
        {
            return (i is Producto p) ? p.Description : string.Empty;
        }
    }
}
