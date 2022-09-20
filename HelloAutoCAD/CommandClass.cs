using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloAutoCAD
{
    public class CommandClass
    {
        [CommandMethod("MyTestCommand")]
        public void RunCommand()
        {

            #region

            /*Document adoc = Application.DocumentManager.MdiActiveDocument; //Если null то открыта стартовая страница

            if (adoc == null)
                return;

            Database db = adoc.Database; //База данных

            ObjectId layerTableId = db.LayerTableId; //Таблица слоёв


            List<string> layersNames = new List<string>();

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                LayerTable layerTable = tr.GetObject(layerTableId, OpenMode.ForRead) as LayerTable;
                foreach (ObjectId layerTableRecordId in layerTable)
                {
                    LayerTableRecord layerTableRecord = tr.GetObject(layerTableRecordId, OpenMode.ForRead) as LayerTableRecord;
                    layersNames.Add(layerTableRecord.Name);
                }
                tr.Commit();
            }

            Editor ed = adoc.Editor;
            foreach (string layerName in layersNames)
            {
                ed.WriteMessage("\n" + layerName);
            }
            */
            #endregion

            Document adoc = Application.DocumentManager.MdiActiveDocument;
            // получаем текущую БД 
            Database db = HostApplicationServices.WorkingDatabase;

            // начинаем транзакцию
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                // получаем ссылку на пространство модели (ModelSpace)
                BlockTableRecord ms = (BlockTableRecord)tr.GetObject(SymbolUtilityServices.GetBlockModelSpaceId(db), OpenMode.ForRead);

                // "пробегаем" по всем объектам в пространстве модели
                foreach (ObjectId id in ms)
                {
                    // приводим каждый из них к типу Entity
                    Entity entity = (Entity)tr.GetObject(id, OpenMode.ForRead);

                    // выводим в консоль слой (entity.Layer), тип (entity.GetType().ToString()) и цвет (entity.Color) каждого объекта
                    /*adoc.Editor.WriteMessage(string.Format("\nLayer:{0}; Type:{1}; Еще что-то {2}\n",
                        entity.Layer,
                        entity.GetType().ToString(),
                        entity.AcadObject.ToString()));*/
                    try
                    {
                        BlockReference blockReference = (BlockReference)entity;        //Выводит в командную строку автокада
                        DynamicBlockReferencePropertyCollection props = blockReference.DynamicBlockReferencePropertyCollection;
                        string prop = null;
                        foreach(DynamicBlockReferenceProperty p in props)
                        {
                            prop += p.PropertyName + p.Value.ToString() + "\n";
                        }

                        adoc.Editor.WriteMessage(string.Format("\nБЛОК АЙДИ:{0};\n" +
                                                               "\nТИП:{1}; \n" +
                                                               "\nНОРМАЛЬ {2};\n" +
                                                               "\nЮНИТ ФАКТОР: {3}\n" +
                                                               "\nСКАЛЯРНЫЕ ФАКТОРЫ {4}\n" + // Вроде бы отличается у элементов повёрнутых на PI/2 (но это не точно)
                                                               "\nECS {5}\n" +
                                                               "\nГЕОМЕТРИЧЕСКИЕ РАСШИРЕНИЯ {6}\n" +
                                                               "\nВИЗИБЛ {7}\n" +
                                                               "\nВИЗИБЛ СТАЙЛ АЙДИ {8}\n" +
                                                               "\nТРАНСФОРМ {9}\n" +
                                                               "\nИМЯ {10}\n" +
                                                               "\nСВОЙСТВА {11}\n" +
                                                               "\n.............................\n",
                            blockReference.BlockId.ToString(),
                            blockReference.BlockName.ToString(),
                            "0",//blockReference.Normal.ToString(),
                            "0",//blockReference.UnitFactor.ToString(),
                            "0",//blockReference.ScaleFactors.ToString(),
                            "0",//blockReference.Ecs.ToString(),
                            "0",//blockReference.GeometricExtents.ToString(),
                            blockReference.Visible.ToString(),
                            blockReference.VisualStyleId.ToString(),
                            blockReference.BlockTransform.ToString(),
                            blockReference.Name.ToString(),
                            prop
                            ));
                    }
                    catch
                    {

                    }
                }
                tr.Commit();
            }

        }     
    }
}
