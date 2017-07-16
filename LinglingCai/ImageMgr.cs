using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace LinglingCai
{
    static class ImageMgr
    {
        private readonly static Random random = new Random();

        private static string repository;
        private static string category;
        private static List<string> files;
        private static int index;
        private static Image lastImage;
        private static Size containerSize;
        private static Category[] categories;

        public static Size ContainerSize
        {
            set
            {
                containerSize = value;
            }
        }
        internal class Category
        {
            public string name;
            public Image image;
        }

        static ImageMgr()
        {
            string PATH1 = ".\\img";
            string PATH2 = "..\\..\\img";
            if (Directory.Exists(PATH1))
            {
                repository = PATH1;
                return;
            }
            
            if (Directory.Exists(PATH2))
            {
                repository = PATH2;
                return;
            }

            MessageBox.Show("出错啦：请把img目录和exe文件放到同一目录下");
            Application.Exit();
        }

        internal static Category[] ListCategories()
        {
            string[] folders = Directory.GetDirectories(repository);

            var ret = new List<Category>();

            for (int i = 0; i < folders.Length; i++)
            {
                string path = folders[i];

                string[] files = Directory.GetFiles(path);
                if (files.Length == 0)
                    continue;

                Image image = Image.FromFile(files[random.Next(files.Length)]);
                string label = path.Substring(path.LastIndexOf('\\') + 1);
                Category c = new Category() { name = label, image = image };
                ret.Add(c);
            }
            return categories = ret.ToArray();
        }

        internal static void InitCategory(string name)
        {
            category = name;

            if (name != null)
            {
                files = LoadFiles(repository + '\\' + name);
            }
            else
            {
                files = new List<string>();

                foreach (Category c in categories)
                {
                    List<string> tmp = LoadFiles(repository + '\\' + c.name);
                    files.AddRange(tmp);
                }
                Shuffle(files);
            }
            index = files.Count - 1;
        }

        internal static Image Prev(out string name)
        {
            index += 2;
            if (index >= files.Count - 1)
                index = files.Count - 1;

            return Next(out name);
        }

        internal static Image Next(out string name)
        {
            if (index < 0)
            {
                MessageBox.Show("全搞定了！");
                InitCategory(category);
            }

            string path = files[index--];
            name = GetName(path);

            if (lastImage != null)
                lastImage.Dispose();
            Image img = Image.FromFile(path);

            if (containerSize != null)
            {
                Image newImg = ResizeImage(img, containerSize);
                if (newImg != img)
                    img.Dispose();
                img = newImg;
            }

            lastImage = img;
            return img;
        }

        public static Image ResizeImage(Image img, Size container)
        {
            Size actual = img.Size;
            Size newSize = new Size();

            float ratioContainer = (float)container.Width / container.Height;
            float ratioImg = (float)actual.Width / actual.Height;

            if (ratioContainer > ratioImg)
            {
                newSize.Height = container.Height;
                newSize.Width = (int)(actual.Width * ((float)newSize.Height / actual.Height));
            }
            else
            {
                newSize.Width = container.Width;
                newSize.Height = (int)(actual.Height * ((float)newSize.Width / actual.Width));
            }

            if (newSize == actual)
                return img;

            return new Bitmap(img, newSize);
        }

        private static string GetName(string path)
        {
            int s = path.LastIndexOf('\\') + 1;
            int e = path.LastIndexOf('.');
            string name;
            if (e < 0)
                name = path.Substring(s);
            else
                name = path.Substring(s, e - s);

            char c = name[name.Length - 1];
            if (Char.IsDigit(c))
                name = name.Substring(0, name.Length - 1);

            return name;
        }
        private static List<string> LoadFiles(string repository)
        {
            string[] all = Directory.GetFiles(repository);

            Shuffle(all);

            var visited = new Dictionary<string, bool>();
            var result = new List<string>();

            foreach (string path in all)
            {
                string name = GetName(path);

                if (visited.ContainsKey(name))
                    continue;

                visited.Add(name, true);
                result.Add(path);
            }

            return result;
        }
        
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
