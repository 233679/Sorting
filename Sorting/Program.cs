using System;
using System.Collections.Generic;
using System.IO;

namespace Sorting
{
    class Program
    {

        static Boolean optimised = false;

        static void Main(string[] args)
        {
            selectSort(args);
        }

        static void selectSort(string[] args)
        {


            int[] testSizes = new int[] { 10, 100, 1000, 2000, 5000, 10000 };
            //int[] testSizes = new int[] { 10, 100, 1000, 2000};

            // Populates the 2 dimentional array with diferent arrays.
            string[][] times = new string[5][];
            times[0] = new string[testSizes.Length];
            times[1] = new string[testSizes.Length];
            times[2] = new string[testSizes.Length];
            times[3] = new string[testSizes.Length];
            times[4] = new string[testSizes.Length];

            // iterates over the given array lengths to sort.
            for (int iteration = 0; iteration < testSizes.Length; iteration++)
            {
                // Creates an array to sort
                string[] names = PopulateArray(testSizes[iteration]);
                string[] temp = new string[names.Length];
                Array.Copy(names, temp, names.Length);
                var watch = new System.Diagnostics.Stopwatch();//storing elapsed time using the built in diagnostics


                // Sorts with insertion.
                watch.Start();
                insertionSort(temp);
                watch.Stop();//stop timer
                times[0][iteration] = watch.ElapsedMilliseconds.ToString();
                watch.Reset();
                Array.Copy(names, temp, names.Length);//reload unsorted data


                // Sorts with bubble.
                watch.Start();
                bubbleSort(temp);
                watch.Stop();//stop timer
                times[1][iteration] = watch.ElapsedMilliseconds.ToString();
                watch.Reset();
                Array.Copy(names, temp, names.Length);//reload unsorted data


                // Sorts with merge.
                watch.Start();
                MergeSort(temp, 0, temp.Length - 1);
                watch.Stop();
                times[2][iteration] = watch.ElapsedMilliseconds.ToString();
                watch.Reset();
                Array.Copy(names, temp, names.Length);


                // Sorts with optimised bubble.
                optimised = true;
                watch.Start();
                bubbleSort(temp);
                watch.Stop();//stop timer
                times[3][iteration] = watch.ElapsedMilliseconds.ToString();
                watch.Reset();
                Array.Copy(names, temp, names.Length);//reload unsorted data


                // Sorts with optimised quick sort.
                watch.Start();
                quickSort(temp, 0, temp.Length - 1);
                watch.Stop();
                times[4][iteration] = watch.ElapsedMilliseconds.ToString();
                watch.Reset();
                Array.Copy(names, temp, names.Length);//reload unsorted data

            }

            // Prints the results to the console.
            for (int i = 0; i < 5; i++) {

                if (i == 0) {Console.Write("Insertion sort: ");}
                else if (i == 1) {Console.Write("Bubble sort: ");}
                else if (i == 2) {Console.Write("Merge sort: ");}
                else if (i == 3) {Console.Write("Bubble sort (Optimised): ");}
                else if (i == 4) {Console.Write("Quick sort: ");}

                foreach (string time in times[i])
                {
                    Console.Write(time);
                    Console.Write("ms ");
                }

                Console.WriteLine();
            }

        }



        static int menu()
        {
            int choice = 0;
            while (choice < 1 || choice > 7)
            {
                Console.WriteLine("1. Print the array so you can see it's random");
                Console.WriteLine("2. Insertion Sort");
                Console.WriteLine("3. Bubble Sort");
                Console.WriteLine("4. Merge Sort");
                Console.WriteLine("5. Optimised Bubble Sort");
                Console.WriteLine("6. Quick Sort");
                Console.WriteLine("7. Exit");
                try
                { choice = Int32.Parse(Console.ReadLine()); }
                catch
                {
                    Console.WriteLine("Try again, this time please enter a number. Enter to continue.");
                    Console.ReadLine();
                }
                Console.Clear();
            }
            return choice;
        }


        static string[] PopulateArray(int qty)
        {

            string[] names = new string[qty];
            Random randFirst = new Random();
            Random randLast = new Random();

            var firstLines = File.ReadAllLines("../../../Resources/firstNames.txt");
            var lastLines = File.ReadAllLines("../../../Resources/surNames.txt");

            for (int i = 0; i < qty; i++)
            {
                names[i] = firstLines[randFirst.Next(0, firstLines.Length)] + " " + lastLines[randLast.Next(0, lastLines.Length)];
            }

            return names;
        }


        static void print_array(string[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                Console.WriteLine(i.ToString() + ". " + arr[i]);
            }
        }

        static Boolean compareStrings(string str1, string str2) //returns true if str1 < str2
        {
            int comp = string.Compare(str1, str2);
            if (comp == -1)
            { return true; }
            else
            { return false; }
        }


        //QUICK SORT ...O(log n)
        static void quickSort(string[] arr, int low, int high)
        {
            if (low < high)
            {

                // pi is partitioning index, arr[p]
                // is now at right place
                int pi = partition(arr, low, high);

                // Separately sort elements before
                // partition and after partition
                quickSort(arr, low, pi - 1);
                quickSort(arr, pi + 1, high);
            }
        }
        static int partition(string[] arr, int low, int high)
        {
            // pivot
            string pivot = arr[high];

            // Index of smaller element and
            // indicates the right position
            // of pivot found so far
            int i = (low - 1);

            for (int j = low; j <= high - 1; j++)
            {

                // If current element is smaller
                // than the pivot
                if (compareStrings(arr[j], pivot))
                {

                    // Increment index of
                    // smaller element
                    i++;
                    swap(arr, i, j);
                }
            }
            swap(arr, i + 1, high);
            return (i + 1);
        }
        static void swap(string[] arr, int i, int j)
        {
            string temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;
        }


        // BUBBLE SORT...O(n^2) because of the nested loops
        static void bubbleSort(string[] arr)
        {

            string temp;
            for (int i = 0; i <= arr.Length - 2; i++)//iterate through the array

            {
                Boolean swapped = false; //only used in the optimised version
                int opt = 0;
                if (optimised) { opt = i; }
                for (int j = 0; j <= arr.Length - 2 - opt; j++) // optimisation - reducing the step each time to avoid looking at bits which we already sorted
                {
                    if (compareStrings(arr[j + 1], arr[j]))
                    {
                        temp = arr[j];
                        arr[j] = arr[j + 1];
                        arr[j + 1] = temp;
                        swapped = true; //only used in the optimised version
                    }

                }
                if (!swapped && optimised) //check to see if the optimised version is being used

                {
                    optimised = false; //reset the boolean
                    return; //we're done - return
                }
            }
        }

        //INSERTION SORT...O(n^2) because of the nested loops
        static void insertionSort(string[] arr)
        {

            string temp; //used for our value that we are going to swap
            Boolean done; // a flag to indicate if we have put the item in the correct place. We can stop when done
            for (int i = 1; i < arr.Length; i++) //NOTE we start at item 1 because item 0 is considered to be in the correct place already
            {
                temp = arr[i];
                done = false;
                for (int j = i - 1; j >= 0 && !done; j--) //loop as long as we still have items to compare (j hasn't reached -1) and we are not done comparing
                {
                    if (compareStrings(temp, arr[j])) //is the current item smaller than the next item in the j loop
                    {
                        arr[j + 1] = arr[j]; //swap - note one less step here then bubble sort because we already put the current item in temp at line 41
                        arr[j] = temp;

                    }
                    else
                    {
                        done = true; //if the current item is bigger than the current item in the j loop it is in the right place and we are done
                    }
                }
            }
        }


        //MERGE SORT .....O(nlog n)
        static public string[] MergeSort(string[] array, int left, int right)//Merge sort recursive routine- needs MergeArray to rebuild.
        {
            if (left < right)
            {
                int middle = left + (right - left) / 2;

                MergeSort(array, left, middle);
                MergeSort(array, middle + 1, right);

                MergeArray(array, left, middle, right);
            }

            return array;
        }
        static public void MergeArray(string[] array, int left, int middle, int right)
        {
            var leftArrayLength = middle - left + 1;
            var rightArrayLength = right - middle;
            var leftTempArray = new string[leftArrayLength];
            var rightTempArray = new string[rightArrayLength];
            int i, j;

            for (i = 0; i < leftArrayLength; ++i)
                leftTempArray[i] = array[left + i];
            for (j = 0; j < rightArrayLength; ++j)
                rightTempArray[j] = array[middle + 1 + j];

            i = 0;
            j = 0;
            int k = left;

            while (i < leftArrayLength && j < rightArrayLength)
            {
                if (compareStrings(leftTempArray[i], rightTempArray[j]))
                {
                    array[k++] = leftTempArray[i++];
                }
                else
                {
                    array[k++] = rightTempArray[j++];
                }
            }

            while (i < leftArrayLength)
            {
                array[k++] = leftTempArray[i++];
            }

            while (j < rightArrayLength)
            {
                array[k++] = rightTempArray[j++];
            }
        }

    }
}
