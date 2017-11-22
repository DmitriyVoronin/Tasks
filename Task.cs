using DocumentWorkflow;
using Ext.Net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace WebApplication.Models
{
    /// <summary>
    /// Задание для подрядных организаций
    /// </summary>
    public class Task
    {

        #region  Свойтсва
        /// <summary>
        /// Id задания
        /// </summary>
        public int TaskId { get; set; }

        /// <summary>
        /// Id подрядной организации
        /// </summary>
        public int ContractorId { get; set; }

        /// <summary>
        /// Название подрядной организации
        /// </summary>
        public string ContractorName { get; set; }

        /// <summary>
        /// Дата начала срока выполнения задания
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Дата окончания срока выполнения задания
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Комментарии представителя подрядной организации по итогам выполнения задания
        /// </summary>
        public String ContractorComments { get; set; }

        /// <summary>
        /// Полное описание задания
        /// </summary>
        public String TaskText { get; set; }


        /// <summary>
        /// Имя объекта строительства
        /// </summary>
        /// 
        public string ObjectBuildingName { get; set; }

        /// <summary>
        /// Номер объекта строительства
        /// </summary>
        public string ObjectBuildingNumber { get; set; }
        /// <summary>
        /// Number + Name
        /// </summary>
        public string ObjectBuildingDescription { get; set; }

        /// <summary>
        /// Плановый объем работ 
        /// </summary>
        public decimal PlanWorkSize { get; set; }

        /// <summary>
        /// ID категории работ
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Название категории работ
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// ID статуса
        /// </summary>
        public int StatusId { get; set; }

        /// <summary>
        /// Название статуса
        /// </summary>
        public string StatusName { get; set; }

        /// <summary>
        /// Название еденицы измерения объема работ 
        /// </summary>
        public string WorkSizeUnitsName { get; set; }
        /// <summary>
        /// ID еденицы измерения объема работ
        /// </summary>
        public int WorkSizeUnitsId { get; set; }
        /// <summary>
        /// Фактический объем работ указанный представителем подрядной организации
        /// </summary>
        public double FactualWorkSizeContractor { get; set; }

        /// <summary>
        /// ID месячного плана
        /// </summary>
        public int MonthPlanId { get; set; }

        /// <summary>
        /// Фактический объем работ указанный контролером
        /// </summary>
        public double FactualWorkSizeController { get; set; }

        /// <summary>
        /// Количество людей указанное представителем подрядной организации
        /// </summary>
        public int PeopleCountContractor { get; set; }

        /// <summary>
        /// Количество людей указанное контролером
        /// </summary>
        public int PeopleCountController { get; set; }

        /// <summary>
        /// Фактический объем работ выполненный по месячному плану к которому относится задание
        /// </summary>
        public double FactualWorkSizeMonthPlan { get; set; }

        /// <summary>
        /// Фактический объем работ выполненный по проектному плану к которому относится задание
        /// </summary>
        public double FactualWorkSizeGlobalPlan { get; set; }

        /// <summary>
        /// 1 - Контролер внес объем работ
        /// 0 - Контролер не внес объем работ
        /// </summary>
        public int ControllerSetWorkSize { get; set; }

        /// <summary>
        /// Id проектного плана 
        /// </summary>
        public int GlobalPlanId { get; set; }

        /// <summary>
        /// Планируемый объем работ в проектном плане к которому относится задание
        /// </summary>
        public double GlobalPlanWorkSize { get; set; }
        /// <summary>
        /// Планируемый объем работ в месячном плане к которому относится задание
        /// </summary>
        public double MonthPlanWorkSize { get; set; }


        #endregion        

        #region  Методы    
        /// <summary>
        /// Получить задание по его ID 
        /// </summary>
        /// <param name="taskId">ID Задания</param>
        /// <returns></returns>
        public static Task getTaskByID(String taskId)
        {
            String filter = "AND TASK_ID = " + taskId;
            List<Task> Tasks = Task.SelectByFilter(filter);
            if (Tasks.Count > 0)
            {
                return Tasks[0];
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// Создать новое задание в указанном месячном плане
        /// </summary>
        /// <param name="monthPlanId">ID месячного плана</param>
        /// <returns></returns>
        public static Task Create(int monthPlanId)
        {
            Task tsk = new Task();
            tsk.MonthPlanId = monthPlanId;
            //По умолчанию задание создается на 1 день
            tsk.StartDate = DateTime.Now;
            tsk.EndDate = DateTime.Now.AddDays(1);
            //Статус новое
            tsk.StatusId = 1;
            tsk.FactualWorkSizeContractor = 0;
            tsk.FactualWorkSizeController = 0;
            //О когда контролер не указал объем работ
            tsk.ControllerSetWorkSize = 0;
            tsk.PeopleCountController = 0;
            tsk.PeopleCountContractor = 0;
            using (Database db = new Database())
            {
                tsk.TaskId = db.GetNextSequenceId("SEQ_GENERAL_REC_ID");
            }
            return tsk;
        }

        /// <summary>
        /// Сохранить задание
        /// </summary>
        public void Save()
        {
            using (Database db = new Database())
            {
                db.CreateOrUpdateRecord("TASKS", "TASK_ID", TaskId,
                new Dictionary<string, object>()
                {
                    {"TASK_ID", TaskId},
                    {"MONTH_PLAN_ID", MonthPlanId},
                    {"START_ID", StartDate},
                    {"END_DATE", EndDate},
                    {"PLAN_WORK_SIZE", PlanWorkSize},
                    {"STATUS_ID",StatusId},
                    {"FACTUAL_WORK_SIZE_CONTROLLER",FactualWorkSizeController},
                    {"FACTUAL_WORK_SIZE_CONTRACTOR",FactualWorkSizeContractor},
                    {"PEOPLE_COUNT_CONTRACTOR", PeopleCountContractor},
                    {"CONTRACTOR_COMMENTS", ContractorComments},
                    {"PEOPLE_COUNT_CONTROLLER", PeopleCountController},
                    {"CONTROLLER_SET_WORK_SIZE", ControllerSetWorkSize}

                });
            }

        }

        /// <summary>
        /// Получить все задания входящие в месячный план по ID месячного плана
        /// </summary>
        /// <param name="mpid">ID месячного плана</param>
        /// <returns></returns>
        public static List<Task> getTasksByMonthId(int MonthPlanId)
        {
            return Task.SelectByFilter(" and t.MONTH_PLAN_ID = " + MonthPlanId);
        }


        /// <summary>
        /// Выбор списка заданий по sql фильтру
        /// </summary>
        /// <param name="filter">sql фильтр обязательно должен начитаться с and</param>
        /// <returns></returns>
        public static List<Task> SelectByFilter(string filter)
        {
            List<Task> TaskList = new List<Task>();
            if (UserGrants.Executor || UserGrants.Controller)
            {
                String sql = @"
                SELECT
                  gp.WorkSizePlan GlobalWorkSize,
                  mp.WorkSize monthWorkSize,
                  AutomaticTaskId,
                  at.MonthPlanId,
                  PlanWorkSize,
                  FactualWorkSizeContractor,
                  FactualWorkSizeController,
                  at.PeopleCount,
                  at.startDate,
                  at.EndDate,
                  s.StatusId,
                  ContractorComments,
                  peopleCountController,
                  at.MonthPlanId,
                  c.Id,
                  c.name,
                  ob.ObjectNumber,
                  ob.ObjectName,
                  s.StatusName,
                  wc.CategoryName,
                  wc.CategoryId,
                  at.REC_DATE lastUpdateDate,
                  mp.WorkName,
                  mp.GlobalPlanId,
                  wc.WorkSizeUnitsName,
                  at.ControllerSetWorkSize,
                  (SELECT
                    SUM(WorkSize) AS ws
                  FROM (SELECT

                    CASE
                      WHEN ControllerSetWorkSize = 1 THEN FactualWorkSizeController
                      ELSE CASE
                          WHEN at2.FactualWorkSizeController = 0 THEN at2.FactualWorkSizeContractor
                          ELSE at2.FactualWorkSizeController
                        END
                    END AS WorkSize

                  FROM AutomaticTasks at2
                  WHERE EDIT_STATE = 0
                  AND MonthPlanId = at.MonthPlanId) AS resTbl)
                  FactualWorkSizeMonthPlan,
                  (SELECT
                    SUM(WorkSize) AS ws
                  FROM (SELECT
                    CASE
                      WHEN ControllerSetWorkSize = 1 THEN FactualWorkSizeController
                      ELSE CASE
                          WHEN at1.FactualWorkSizeController = 0 THEN at1.FactualWorkSizeContractor
                          ELSE at1.FactualWorkSizeController
                        END
                    END AS WorkSize

                  FROM AutomaticTasks at1
                  LEFT JOIN MonthPlans mp1
                    ON mp1.MonthPlanId = at1.MonthPlanId
                    AND mp1.EDIT_STATE = 0
                  WHERE at1.EDIT_STATE = 0
                  AND mp1.GlobalPlanId = mp.GlobalPlanId) AS resTbl)
                  FactualWorkSizeGlobalPlan
                FROM AutomaticTasks at
                JOIN MonthPlans mp
                  ON mp.MonthPlanId = at.MonthPlanId
                  AND mp.EDIT_STATE = 0
                JOIN GlobalPlans gp
                  ON gp.GlobalPlanId = mp.GlobalPlanId
                  AND gp.EDIT_STATE = 0
                LEFT JOIN WorkCategories wc
                  ON gp.CategoryId = wc.CategoryId
                  AND wc.EDIT_STATE = 0
                LEFT JOIN contractors c
                  ON c.Id = gp.ContractorId
                LEFT JOIN vObjectBuilding ob
                  ON gp.ObjectBuildingNumber = ob.ObjectNumber
                LEFT JOIN Statuses s
                  ON s.StatusId = at.StatusId

                WHERE at.EDIT_STATE = 0 {condition}";

                if (UserGrants.Executor)
                {

                    List<TaskExecutor> te = TaskExecutor.SelectByFilter(UserAuthorization.UserName);
                    if (te.Count  > 0)
                        filter = filter + " and contractorId = " + te[0].ContractorId;
                    else
                    {
                        return TaskList;
                    }
                }
                sql = sql.Replace("{condition}", filter);
                using (Database db = new Database())
                {
                    DataTable tbl = db.SelectSqlQuery(sql);
                    foreach (DataRow item in tbl.Rows)
                    {
                        Task tf = new Task();
                        tf.GlobalPlanWorkSize = Convert.ToDouble(item["GlobalWorkSize"]);
                        tf.MonthPlanWorkSize = Convert.ToDouble(item["monthWorkSize"]);
                        tf.CategoryName = item["CategoryName"].ToString();
                        tf.CategoryId = Convert.ToInt32(item["CategoryId"]);
                        tf.WorkSizeUnitsName = item["WorkSizeUnitsName"].ToString();                        
                        tf.ContractorName = item["name"].ToString();
                        tf.ObjectBuildingName = item["ObjectName"].ToString();
                        tf.ObjectBuildingNumber = item["ObjectNumber"].ToString();
                        tf.ObjectBuildingDescription = item["ObjectNumber"].ToString() + "-" + item["ObjectName"].ToString();
                        tf.TaskId = Convert.ToInt32(item["AutomaticTaskId"]);
                        tf.MonthPlanId = Convert.ToInt32(item["MonthPlanId"]);
                        tf.PlanWorkSize = Convert.ToDecimal(item["PlanWorkSize"]);
                        tf.FactualWorkSizeController = Convert.ToDouble(item["FactualWorkSizeController"]);
                        tf.FactualWorkSizeContractor = Convert.ToDouble(item["FactualWorkSizeContractor"]);
                        tf.PeopleCountContractor = Convert.ToInt32(item["PeopleCount"]);
                        tf.StartDate = Convert.ToDateTime(item["startDate"]);
                        tf.EndDate = Convert.ToDateTime(item["EndDate"]);
                        tf.StatusName = item["StatusName"].ToString();
                        tf.StatusId = Convert.ToInt32(item["StatusId"]);
                        tf.TaskText = item["WorkName"].ToString();
                        tf.PeopleCountController = Convert.ToInt32(item["peopleCountController"]);
                        tf.ContractorComments = item["ContractorComments"].ToString();
                        tf.ControllerSetWorkSize = Convert.ToInt32(item["ControllerSetWorkSize"]);
                        tf.GlobalPlanId = Convert.ToInt32(item["GlobalPlanId"]);
                        tf.FactualWorkSizeMonthPlan = Convert.ToDouble(item["FactualWorkSizeMonthPlan"]);                        
                        tf.FactualWorkSizeGlobalPlan = Convert.ToDouble(item["FactualWorkSizeGlobalPlan"]);                        
                        tf.ContractorId = Convert.ToInt32(item["Id"]);

                        TaskList.Add(tf);
                    }

                }
            }
            return TaskList;
        }     

        /// <summary>
        /// Возвращает true если исполнитель может редактировать задание
        /// </summary>
        /// <returns></returns>
        public bool ExecutorCanEditTask()
        {
            //Если задание в статусе новое или на проверке
            if ((StatusId == 1) && UserGrants.Executor) { return true; }
            return false;
        } 
        
        /// <summary>
        /// Возвращает true если в системе есть задания по которым не отчитался исполнитель 
        /// </summary>
        /// <returns></returns>
        public static bool checkUnreportedTasks()
        {
            List<Task> list = Task.SelectByFilter(String.Format(" AND at.StatusId = 1 and at.FIRST_REC_DATE >= CONVERT(DATETIME, '{0}', 120)", DateTime.Today.ToString("yyyy-MM-dd HH:mm:ss")));
            if (list.Count > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Создает задания на определенный период с автоматическим расчетои планового объема работ
        /// </summary>
        /// <param name="start">Дата начала периода</param>
        /// <param name="end">Дата окончания периода</param>
        /// <returns></returns>
        public static void MakeTasksPeriod(DateTime start, DateTime end)
        {
            //количество дней для которых необходимо создать задания
            TimeSpan dayDiff = end - start;
            //Временная переменная для даты начала периода
            DateTime tempDate = start;
            //Номер дня для которого расчитывается плановый показатель
            int DayNumForCreateTask = Convert.ToInt32(String.Format("{0:dd}", start));
            for (int i = 0; i < dayDiff.Days + 1; i++)
            {

                DateTime CurDate = DateTime.Now;          
                //Текущий год
                String planYear = String.Format(CultureInfo.CreateSpecificCulture("ru-RU"), "{0:yyyy}", CurDate);
                //Выбор месячных планов включающих эту дату
                List<MonthPlan> monthPlans = MonthPlan.GetMonthPlansIncludingDate(tempDate);

                foreach (MonthPlan mp in monthPlans)
                {
                    Task tf = Task.Create(mp.MonthPlanId);
                    int CurMonthNumber = Convert.ToInt32(String.Format("{0:MM}", CurDate));
                    int dayInMonth = DateTime.DaysInMonth(Convert.ToInt32(planYear), CurMonthNumber);
                    //double MonthWorkSize = Convert.ToDouble(item["WorkSize"]);
                    //string WorkSizeUnits = mp.WorkSizeUnitsName;

                    tf.TaskText = mp.WorkName;                    
                    //Если в месячном плане необходимо автоматически расчитывать плановые показатели и создавать задания
                    if (mp.CreateTaskAutomatically)
                    {                        
                        if (mp.WorkSizePlan != 0)
                        {
                            tf.PlanWorkSize = (mp.WorkSizePlan) / (dayInMonth);                            
                        }
                        else
                        {
                            tf.PlanWorkSize = 0;
                        }

                        tf.StartDate = tempDate;
                        tf.EndDate = tempDate.AddDays(1);
                        //Проверка, было ли создано задания для этого месячного плана за такой же период
                        if (CheckTaskAlreadyCreted(tf))
                        {
                            tf.TaskId = db.GetNextSequenceId("SEQ_GENERAL_REC_ID");
                            tf.Save();
                        }

                    }                    
                }
                
                tempDate = tempDate.AddDays(1);
                DayNumForCreateTask++;
            }
                      
        }

        /// <summary>
        /// Возвращает true если задание за такой же период уже создано в месячном плане
        /// </summary>
        /// <param name="task">Задание</param>
        /// <returns></returns>
        public static bool CheckTaskAlreadyCreted(Task task)
        {
            String sql = @"SELECT 
                            at.startDate,
                            at.EndDate,
                            gp.CategoryId,
                            mp.WorkName,
                            c.name
                            FROM AutomaticTasks at 
                            JOIN MonthPlans mp ON at.MonthPlanId = mp.MonthPlanId AND mp.EDIT_STATE = 0
                            JOIN GlobalPlans gp ON gp.GlobalPlanId = gp.GlobalPlanId AND gp.EDIT_STATE = 0
                            JOIN WorkCategories wc ON wc.CategoryId = gp.CategoryId AND wc.EDIT_STATE = 0
                            JOIN contractors c ON c.Id = gp.ContractorId
                            WHERE at.EDIT_STATE = 0 and
                            CONVERT(DATE, at.startDate, 103) =  CONVERT(DATE, '" + task.StartDate.ToString() + @"', 103) and
                            CONVERT(DATE, at.EndDate, 103) =  CONVERT(DATE, '" + task.EndDate.ToString() + @"', 103) and
                            at.MonthPlanId = " + task.MonthPlanId;
            using (Database db = new Database())
            {
                DataTable tbl = db.SelectSqlQuery(sql);
                if (tbl.Rows.Count > 0)
                    return false;

            }
            return true;
        }

        /// <summary>
        /// Перевод всех заданий созданных до сегодня в определнный статус 
        /// </summary>
        /// <param name="statusId">ID статуса в который необходимо перевести</param>
        public static void ChangeStatusIdForAllOldTasks(int statusId)
        {
            List<Task> list = Task.SelectByFilter(String.Format("and at.startDate < CONVERT(DATETIME, '{0}', 103) and at.StatusId <> 3", DateTime.Today.AddDays(1)));
            foreach (Task tf in list)
            {
                tf.StatusId = statusId;
                tf.Save();
            }
        }

        /// <summary>
        /// Перевести все задания за определенный период в статус подтверждено 
        /// </summary>
        /// <param name="startDate">Дата начала периода</param>
        /// <param name="endDate">Дата окончания периода</param>
        public static void AcseptTasksForPeriod(DateTime startDate, DateTime endDate)
        {
            List<Task> list = Task.SelectByFilter("and at.startDate <= CONVERT(DATETIME, '" + startDate + "', 103) and at.startDate >= CONVERT(DATETIME, '" + endDate + "', 103) and at.StatusId <> 3");
            foreach (Task tf in list)
            {
                tf.StatusId = 3;
                tf.Save();
            }
        }
   

        /// <summary>
        ///  Заблокировать исполнителям изменение данных в заявки созданные до сгодешнего дня
        /// </summary>
        public static void BlockExecutorsInsertInfo()
        {
            ChangeStatusIdForAllOldTasks(2);
        }              

        //Удалить задание
        public void Delete()
        {
            using (Database db = new Database())
            {
                db.DeleteRecord("AutomaticTasks", "AutomaticTaskId", TaskId);
            }
        }

        #endregion

    }
}