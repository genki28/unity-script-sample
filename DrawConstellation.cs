using System.Linq;
using UnityEngine;

public class DrawConstellation : MonoBehaviour
{
    static float SpaceSize = 1500.0f; // �������̔��a
    static float StarBaseSize = 8.0f; // ���̑傫���̊

    [SerializeField]
    GameObject starPrefab; // ���̃v���n�u
    [SerializeField]
    GameObject linePrefab; // �������̃v���n�u
    [SerializeField]
    GameObject namePrefab;�@// �������̃v���n�u

    public ConstellationData ConstellationData { get; set; } // �`�悷�鐯���f�[�^

    GameObject linesParent; // ���C�����܂Ƃ߂�Q�[���I�u�W�F�N�g

    // ���C�����܂Ƃ߂�̃Q�[���I�u�W�F�N�g�̃v���p�e�B
    public GameObject LinesParent { get { return linesParent; } }

    void Start()
    {
        // GameObject �̖��O�𐯍����ɕύX
        if (ConstellationData.Name != null)
        {
            gameObject.name = ConstellationData.Name.Name;
        }

        // �f�[�^���琯�����쐬
        CreateConstellation();
    }

    // �����̍쐬
    void CreateConstellation()
    {
        // ���X�g���琯���쐬
        foreach (var star in ConstellationData.Stars)
        {
            // ���̍쐬
            var starObject = CreateStar(star);
            // �����̎q���ɐڑ�
            starObject.transform.SetParent(transform, false);
        }

        if (ConstellationData.Lines != null)
        {
            // �������̐e���쐬
            linesParent = new GameObject("Lines");
            // �����̎q���ɐڑ�
            linesParent.transform.SetParent(transform, false);
            var parent = linesParent.transform;

            // ���X�g���琯�������쐬
            foreach (var line in ConstellationData.Lines)
            {
                // �������̍쐬
                var lineObject = CreateLine(line);
                // �������̐e�̎q���ɐڑ�
                lineObject.transform.SetParent(parent, false);
            }
        }

        if (ConstellationData.Name != null)
        {
            // ���������쐬
            var nameObject = CreateName(ConstellationData.Name, ConstellationData.Position);
            // �����̎q���ɐڑ�
            nameObject.transform.SetParent(transform, false);
        }
    }

    // ���̍쐬
    GameObject CreateStar(StarData starData)
    {
        // ���̃v���n�u����C���X�^���X�쐬
        var star = Instantiate(starPrefab);
        var starTrans = star.transform;

        // ���̌���������։�]������
        starTrans.localRotation = Quaternion.Euler(starData.Declination, starData.RightAscension, 0.0f);
        // ���̖��O��HIP�ԍ��ɂ���
        star.name = string.Format("{0}", starData.Hip);

        var child = starTrans.GetChild(0);
        // �q���̋��̈ʒu��V���̈ʒu�ֈړ�������
        child.transform.localPosition = new Vector3(0.0f, 0.0f, SpaceSize);

        // �������𐯂̃T�C�Y�ɂ���
        var size = StarBaseSize - starData.ApparentMagnitude;
        child.transform.localScale = new Vector3(size, size, size);

        // Renderer�̎擾
        var meshRanderer = child.GetComponent<Renderer>();
        var color = Color.white;

        // ���̃J���[�^�C�v�ɂ��F��ݒ肷��
        switch (starData.ColorType)
        {
            case "0": // ��
                color = Color.blue;
                break;
            case "B": // ��
                color = Color.Lerp(Color.blue, Color.white, 0.5f);
                break;
            default:
            case "A": // ��
                color = Color.white;
                break;
            case "F": // ����
                color = Color.Lerp(Color.white, Color.yellow, 0.5f);
                break;
            case "G": // ��
                color = Color.yellow;
                break;
            case "K": // ��
                color = new Color(243.0f / 255.0f, 152.0f / 255.0f, 0.0f);
                break;
            case "M": // ��
                color = new Color(200.0f / 255.0f, 10.0f / 255.0f, 0.0f);
                break;
        }

        // �}�e���A���ɐF��ݒ肷��
        meshRanderer.material.SetColor("_Color", color);

        return star;
    }

    // �������̍쐬
    GameObject CreateLine(ConstellationLineData lineData)
    {
        // �n�_�̐��̏����擾
        var start = GetStar(lineData.StartHip);
        // �I�_�̐��̏����擾
        var end = GetStar(lineData.EndHip);
        // �������̃v���n�u����C���X�^���X�쐬
        var line = Instantiate(linePrefab);
        // LineRenderer�̎擾
        var lineRenderer = line.GetComponent<LineRenderer>();

        // LineRenderer�̎n�_�ƏI�_�̈ʒu��o�^(���̌���������։�]��������A�V���̈ʒu�܂ňړ�������)
        lineRenderer.SetPosition(0, Quaternion.Euler(start.Declination, start.RightAscension, 0.0f) * new Vector3(0.0f, 0.0f, SpaceSize));
        lineRenderer.SetPosition(1, Quaternion.Euler(end.Declination, end.RightAscension, 0.0f) * new Vector3(0.0f, 0.0f, SpaceSize));

        return line;
    }

    // StarData�̃f�[�^����
    StarData GetStar(int hip)
    {
        // ����HIP�ԍ�������
        return ConstellationData.Stars.FirstOrDefault(s => hip == s.Hip);
    }

    // �������̍쐬
    GameObject CreateName(ConstellationNameData nameData, ConstellationPositionData positionData)
    {
        // �������̃v���n�u����C���X�^���X�쐬
        var text = Instantiate(namePrefab);
        var textTrans = text.transform;

        // ���̌���������։�]������
        textTrans.localRotation = Quaternion.Euler(positionData.Declination, positionData.RightAscension, 0.0f);
        text.name = nameData.Name;

        // �q����3D Text�̈ʒu��V���̈ʒu�ֈړ�������
        var child = textTrans.GetChild(0);
        child.transform.localPosition = new Vector3(0.0f, 0.0f, SpaceSize);

        // TextMesh���擾���āA�����̖��O�ɕύX����
        var textMesh = child.GetComponent<TextMesh>();
        textMesh.text = string.Format("{0}��", nameData.JapaneseName);

        return text;
    }
}