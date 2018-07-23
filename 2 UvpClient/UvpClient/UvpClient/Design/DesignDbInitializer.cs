using System;
using System.Collections.Generic;
using UvpClient.Models;

namespace UvpClient.Design {
    public class DesignDbInitializer {
        private readonly DesignDataContext _designDataContext;

        public DesignDbInitializer(DesignDataContext designDataContext) {
            _designDataContext = designDataContext;
        }

        public void Initialize() {
            _designDataContext.Database.EnsureCreated();

            var teachingClazzes = new[] {
                new TeachingClazz {Name = "uvp.17.1"},
                new TeachingClazz {Name = "uvp.17.2"},
                new TeachingClazz {Name = "uvp.17.3"},
                new TeachingClazz {Name = "uvp.17.4"}
            };
            foreach (var teachingClazz in teachingClazzes)
                _designDataContext.TeachingClazz.Add(teachingClazz);
            _designDataContext.SaveChanges();

            var clazzes = new[] {
                new Clazz {Name = "计算机0208班"}, new Clazz {Name = "计算机0209班"},
                new Clazz {Name = "计算机0210班"}, new Clazz {Name = "计算机0211班"},
                new Clazz {Name = "计算机0212班"}, new Clazz {Name = "计算机0213班"},
                new Clazz {Name = "计算机0214班"}, new Clazz {Name = "计算机0215班"}
            };
            foreach (var clazz in clazzes)
                _designDataContext.Clazz.Add(clazz);
            _designDataContext.SaveChanges();

            var groups = new[] {
                new Group {
                    TeachingClazzID = 1, Number = 1, Name = "uvp.17.1.group.1"
                },
                new Group {
                    TeachingClazzID = 1, Number = 2, Name = "uvp.17.1.group.2"
                },
                new Group {
                    TeachingClazzID = 2, Number = 1, Name = "uvp.17.2.group.1"
                },
                new Group {
                    TeachingClazzID = 2, Number = 2, Name = "uvp.17.2.group.2"
                },
                new Group {
                    TeachingClazzID = 3, Number = 1, Name = "uvp.17.3.group.1"
                },
                new Group {
                    TeachingClazzID = 3, Number = 2, Name = "uvp.17.3.group.2"
                },
                new Group {
                    TeachingClazzID = 4, Number = 1, Name = "uvp.17.4.group.1"
                },
                new Group
                    {TeachingClazzID = 4, Number = 2, Name = "uvp.17.4.group.2"}
            };
            foreach (var group in groups)
                _designDataContext.Group.Add(group);
            _designDataContext.SaveChanges();

            var students = new[] {
                new Student {
                    StudentId = "20023101", Name = "张引", ClazzID = 1,
                    TeachingClazzID = 1, GroupID = 1
                },
                new Student {
                    StudentId = "20023102", Name = "张引", ClazzID = 1,
                    TeachingClazzID = 1, GroupID = 1
                },
                new Student {
                    StudentId = "20023103", Name = "张引", ClazzID = 2,
                    TeachingClazzID = 1, GroupID = 2
                },
                new Student {
                    StudentId = "20023104", Name = "张引", ClazzID = 2,
                    TeachingClazzID = 1, GroupID = 2
                },
                new Student {
                    StudentId = "20023105", Name = "张引", ClazzID = 3,
                    TeachingClazzID = 2, GroupID = 3
                },
                new Student {
                    StudentId = "20023106", Name = "张引", ClazzID = 3,
                    TeachingClazzID = 2, GroupID = 3
                },
                new Student {
                    StudentId = "20023107", Name = "张引", ClazzID = 4,
                    TeachingClazzID = 2, GroupID = 4
                },
                new Student {
                    StudentId = "20023108", Name = "张引", ClazzID = 4,
                    TeachingClazzID = 2, GroupID = 4
                },
                new Student {
                    StudentId = "20023109", Name = "张引", ClazzID = 5,
                    TeachingClazzID = 3, GroupID = 5
                },
                new Student {
                    StudentId = "20023110", Name = "张引", ClazzID = 5,
                    TeachingClazzID = 3, GroupID = 5
                },
                new Student {
                    StudentId = "20023111", Name = "张引", ClazzID = 6,
                    TeachingClazzID = 3, GroupID = 6
                },
                new Student {
                    StudentId = "20023112", Name = "张引", ClazzID = 6,
                    TeachingClazzID = 3, GroupID = 6
                },
                new Student {
                    StudentId = "20023113", Name = "张引", ClazzID = 7,
                    TeachingClazzID = 4, GroupID = 7
                },
                new Student {
                    StudentId = "20023114", Name = "张引", ClazzID = 7,
                    TeachingClazzID = 4, GroupID = 7
                },
                new Student {
                    StudentId = "20023115", Name = "张引", ClazzID = 8,
                    TeachingClazzID = 4, GroupID = 8
                },
                new Student {
                    StudentId = "20023116", Name = "张引", ClazzID = 8,
                    TeachingClazzID = 4
                }
            };
            foreach (var student in students)
                _designDataContext.Student.Add(student);
            _designDataContext.SaveChanges();

            var homeworks = new[] {
                new Homework {
                    Subject = "h1", Description = "h1",
                    CreationDate = DateTime.MinValue,
                    Deadline = DateTime.MaxValue, Type = HomeworkType.Group
                },
                new Homework {
                    Subject = "h2", Description = "h2",
                    CreationDate = DateTime.MinValue,
                    Deadline = DateTime.MaxValue, Type = HomeworkType.Student
                }
            };
            foreach (var homework in homeworks)
                _designDataContext.Homework.Add(homework);
            _designDataContext.SaveChanges();

            var groupAssignments = new[] {
                new GroupAssignment {GroupID = 1, HomeworkID = 1},
                new GroupAssignment {GroupID = 2, HomeworkID = 1}
            };
            foreach (var groupAssignment in groupAssignments)
                _designDataContext.GroupAssignment.Add(groupAssignment);
            _designDataContext.SaveChanges();

            var studentAssignments = new[] {
                new StudentAssignment {StudentID = 1, HomeworkID = 2},
                new StudentAssignment {StudentID = 2, HomeworkID = 2},
                new StudentAssignment {StudentID = 3, HomeworkID = 2},
                new StudentAssignment {StudentID = 4, HomeworkID = 2}
            };
            foreach (var studentAssignment in studentAssignments)
                _designDataContext.StudentAssignment.Add(studentAssignment);
            _designDataContext.SaveChanges();

            var questionnaires = new[] {
                new Questionnaire {
                    Title = "q1", Question = "q1",
                    OptionCollection =
                        new List<string> {"good", "bad"}.AsReadOnly(),
                    Type = QuestionnaireType.ExclusiveChoice,
                    CreationDate = DateTime.MinValue,
                    Deadline = DateTime.MaxValue
                },
                new Questionnaire {
                    Title = "q2", Question = "q2",
                    OptionCollection = new List<string> {"good", "bad", "gay"}
                        .AsReadOnly(),
                    Type = QuestionnaireType.MultipleChoice,
                    CreationDate = DateTime.MinValue,
                    Deadline = DateTime.MaxValue
                }
            };
            foreach (var questionnaire in questionnaires)
                _designDataContext.Questionnaire.Add(questionnaire);
            _designDataContext.SaveChanges();

            var votes = new[] {
                new Vote {QuestionnaireID = 1, StudentID = 1},
                new Vote {QuestionnaireID = 1, StudentID = 2},
                new Vote {QuestionnaireID = 2, StudentID = 1},
                new Vote {QuestionnaireID = 2, StudentID = 2}
            };
            foreach (var vote in votes)
                _designDataContext.Vote.Add(vote);
            _designDataContext.SaveChanges();
        }
    }
}