
CREATE TABLE [dbo].[Report] (
    [Id]           INT      NOT NULL,
    [DateUploaded] DATETIME CONSTRAINT [DF_Report_DateUploaded] DEFAULT (getutcdate()) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Employee] (
    [Id]       INT         NOT NULL,
    [JobGroup] VARCHAR (5) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Pay] (
    [Id]          INT        IDENTITY (1, 1) NOT NULL,
    [EmployeeId]  INT        NOT NULL,
    [ReportId]    INT        NOT NULL,
    [DateWorked]  DATETIME   NOT NULL,
    [HoursWorked] FLOAT (53) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Pay_Employee_Id] FOREIGN KEY ([EmployeeId]) REFERENCES [dbo].[Employee] ([Id]),
    CONSTRAINT [FK_Pay_Report_Id] FOREIGN KEY ([ReportId]) REFERENCES [dbo].[Report] ([Id])
);