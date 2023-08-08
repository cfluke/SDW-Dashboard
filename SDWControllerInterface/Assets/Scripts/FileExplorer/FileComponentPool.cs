using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace FileExplorer
{
    public class FileComponentPool : MonoBehaviour
    {
        [SerializeField] private GameObject filePrefab;
        [SerializeField] private int poolSize;
        private Queue<GameObject> _objectPool;

        void Start()
        {
            _objectPool = new Queue<GameObject>();

            for (int i = 0; i < poolSize; i++)
            {
                GameObject file = Instantiate(filePrefab, transform);
                file.SetActive(false);
                _objectPool.Enqueue(file);
            }
        }

        public FileComponent Borrow(string directory, FileComponentType fileComponentType, Action<string> callback)
        {
            GameObject fileObject = _objectPool.Dequeue();

            Button button = fileObject.GetComponent<Button>();
            button.onClick.AddListener(() => callback.Invoke(directory));
            
            FileComponent file = fileObject.GetComponent<FileComponent>();
            file.SetName(Path.GetFileName(directory));
            file.SetIcon(fileComponentType);

            // enable and borrow
            fileObject.SetActive(true);
            return file;
        }

        public void Return(FileComponent file)
        {
            GameObject fileObject = file.gameObject;
            fileObject.SetActive(false);
            _objectPool.Enqueue(fileObject);
        }
    }
}