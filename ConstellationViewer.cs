using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConstellationViewer : MonoBehaviour
{
    // ����CSV�f�[�^
    [SerializeField]
    TextAsset starDataCSV;
    [SerializeField]
    TextAsset starMajorDataCSV;
    [SerializeField]
    TextAsset constellationNameDataCSV;
    [SerializeField]
    TextAsset constellationPositionDataCSV;
    [SerializeField]
    TextAsset constellationLineDataCSV;

    [SerializeField]
    GameObject constellationPrefab; // �����̃v���n�u

    // �����f�[�^
    List<StarData> starData;
    List<StarMajorData> starMajorData;
    List<ConstellationNameData> constellationNameData;
    List<ConstellationPositionData> constellationPositionData;
    List<ConstellationLineData> constellationLineData;

    // �������s���������̃f�[�^
    List<ConstellationData> constellationData;

    void Start()
    {
        // CSV�f�[�^�̓ǂݍ���
        LoadCSV();

        // �����f�[�^�̐���
        ArrangementData();

        // �����̍쐬
        CreateConstellation();
    }

    // CSV�f�[�^�̓ǂݍ���
    void LoadCSV()
    {
        starData = CsvLoader<StarData>.LoadData(starDataCSV);
        starMajorData = CsvLoader<StarMajorData>.LoadData(starMajorDataCSV);
        constellationNameData = CsvLoader<ConstellationNameData>.LoadData(constellationNameDataCSV);
        constellationPositionData = CsvLoader<ConstellationPositionData>.LoadData(constellationPositionDataCSV);
        constellationLineData = CsvLoader<ConstellationLineData>.LoadData(constellationLineDataCSV);
    }

    // �����f�[�^�̐���
    void ArrangementData()
    {
        // ���f�[�^�𓝍�
        MergeStarData();

        constellationData = new List<ConstellationData>();

        // ���������琯���ɕK�v�ȃf�[�^�����W
        foreach (var name in constellationNameData)
        {
            constellationData.Add(CollectConstellationData(name));
        }

        // �����Ɏg���Ă��Ȃ����̎��W
        var data = new ConstellationData();
        data.Stars = starData.Where(starData => starData.UseConstellation == false).ToList();
        constellationData.Add(data);
    }

    // ���f�[�^�𓝍�
    void MergeStarData()
    {
        // ����g�p����K�v�Ȑ��𔻕ʂ���
        foreach (var star in starMajorData)
        {
            // �����f�[�^�����邩�H
            var data = starData.FirstOrDefault(s => star.Hip == s.Hip);
            if (data != null)
            {
                // �����f�[�^���������ꍇ�A�ʒu�f�[�^���X�V����
                data.RightAscension = star.RightAscension;
                data.Declination = star.Declination;
            } 
            else
            {
                // �����f�[�^���Ȃ��ꍇ�A5������薾�邢�̂ł���΁A���X�g�ɒǉ�����
                if (star.ApparentMagnitude <= 5.0f)
                {
                    starData.Add(star);
                }
            }
        }
    }

    // �����f�[�^�̎��W
    ConstellationData CollectConstellationData(ConstellationNameData name)
    {
        var data = new ConstellationData();

        // �����̖��O�o�^
        data.Name = name;

        // ����ID���������̂�o�^
        data.Position = constellationPositionData.FirstOrDefault(s => name.Id == s.Id);

        // �����̗��̂��������̂�o�^
        data.Lines = constellationLineData.Where(starData => name.Summary == starData.Name).ToList();

        // ���������g�p���Ă��鐯��o�^
        data.Stars = new List<StarData>();
        foreach (var line in data.Lines)
        {
            var start = starData.FirstOrDefault(s => s.Hip == line.StartHip);
            data.Stars.Add(start);
            var end = starData.FirstOrDefault(s => s.Hip == line.EndHip);
            data.Stars.Add(end);

            // �����Ŏg�p����鐯
            start.UseConstellation = end.UseConstellation = true;
        }

        return data;
    }

    // �����̍쐬
    void CreateConstellation()
    {
        // �e�������쐬
        foreach (var data in constellationData)
        {
            var constellation = Instantiate(constellationPrefab);
            var drawConstellation = constellation.GetComponent<DrawConstellation>();

            drawConstellation = constellation.GetComponent<DrawConstellation>();

            drawConstellation.ConstellationData = data;

            // �����̎q���ɂ���
            constellation.transform.SetParent(transform, false);
        }
    }
}