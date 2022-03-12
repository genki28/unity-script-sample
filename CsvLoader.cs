using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CsvLoader<TCsvData> where TCsvData : CsvData, new()
{
    // TextAsset�f�[�^�̓ǂݍ���
    public static List<TCsvData> LoadData(TextAsset csvText)
    {
        var data = new List<TCsvData>();
        var reader = new StringReader(csvText.text);

        // 1�s���f�[�^�̍Ō�܂ŏ������s��
        while (reader.Peek() > -1)
        {
            // 1�s�ǂݍ���
            var line = reader.ReadLine();
            // �f�[�^�쐬
            var csvData = new TCsvData();
            // ,�ŋ�؂����f�[�^�̔z����쐬���ăf�[�^��o�^����
            csvData.SetData(line.Split(','));
            // ���X�g�ɓo�^
            data.Add(csvData);
        }
        return data;
    }
}
