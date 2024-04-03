using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.Collections.Generic;

namespace Novel
{
    using Cysharp.Threading.Tasks;
    using Novel.Command;

    [CustomEditor(typeof(FlowchartExecutor))]
    public class FlowchartExecutorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("�t���[�`���[�g�G�f�B�^���J��"))
            {
                EditorWindow.GetWindow<FlowchartEditorWindow>("Flowchart Editor");
            }

            base.OnInspectorGUI();

            EditorGUILayout.Space(10);

            EditorGUILayout.LabelField(
                "��������\n" +
                "���̃{�^�����畡�������Ă��������B����Ȃ��ƃo�O��܂�\n" +
                "�������ʂɕ������Ă��܂��Ă��A���̂܂܍폜����Α��v�ł�"
                , EditorStyles.wordWrappedLabel);

            EditorGUILayout.Space(10);

            if (GUILayout.Button("��������"))
            {
                var flowchartExecutor = target as FlowchartExecutor;
                var copiedFlowchartExecutor = Instantiate(flowchartExecutor);
                copiedFlowchartExecutor.name = "Flowchart(Copied)";
                var flowchart = copiedFlowchartExecutor.Flowchart;

                var copiedCmdList = new List<CommandData>();
                foreach (var cmdData in flowchart.GetCommandDataList())
                {
                    var copiedCmdData = Instantiate(cmdData);
                    var cmd = copiedCmdData.GetCommandBase();
                    if (cmd != null)
                    {
                        cmd.SetFlowchart(flowchart);
                    }
                    copiedCmdList.Add(copiedCmdData);
                }
                flowchart.SetCommandDataList(copiedCmdList);
            }

            EditorGUILayout.Space(10);

            EditorGUILayout.LabelField(
                "����CSV�̓��o�͂ɂ���\n" +
                "�G�N�X�|�[�g����ƁA�V�[�����̑SFlowchartExecutor�̃R�}���h�f�[�^���������܂�܂�\n" +
                "�C���|�[�g����ƁA�o�͂����`���Ńf�[�^����荞�ނ��Ƃ��ł��܂�\n" +
                "Excel�ł̃f�[�^�`���ɏ����Ă��܂��̂ŁA���̂܂܈����܂�\n" +
                "\n" +
                "�o�͂���f�[�^����͂���f�[�^��ݒ肵�����ꍇ�́ACommandBase����CSVContent1��CSVContent2�v���p�e�B��h���R�}���h���ɃI�[�o�[���C�h���Ă��������B�Q�b�^�[�ƃZ�b�^�[�͑��ݕϊ��𐄏����܂�\n" +
                "\n" +
                "�܂��A�c�̗�̃R�}���h�����R�ɑ��₷���Ƃ��ł��܂��B�R�}���h�����삷��ꍇ�A�N���X����\"�`Command\"�Ƃ��Ă�������\n" +
                "���łɂ���R�}���h��CSV��������@�\�͎������Ă��܂���B�Ƃ肠���������ɂ������ꍇ��\"<Null>\"�܂��͒P��\"Null\"�����Ă�������\n" +
                "\n" +
                "���ӓ_�Ƃ��āA�t�@�C�����͕ύX���Ă����܂��܂��񂪁ACSV����1�s�ڂ�2�s�ڂ̃f�[�^�͊�{�I�ɕς��Ȃ��ł�������"
                , EditorStyles.wordWrappedLabel);

            EditorGUILayout.Space(10);

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("CSV�`���ŃG�N�X�|�[�g����"))
                {
                    var sceneName = SceneManager.GetActiveScene().name;
                    FlowchartCSVIO.ExportFlowchartCommandDataAsync(
                        sceneName, FlowchartCSVIO.FlowchartType.Executor).Forget();
                }
                if (GUILayout.Button("CSV���C���|�[�g����"))
                {
                    var sceneName = SceneManager.GetActiveScene().name;
                    FlowchartCSVIO.ImportFlowchartCommandDataAsync(
                        sceneName, FlowchartCSVIO.FlowchartType.Executor).Forget();
                }
            }
        }
    }
}