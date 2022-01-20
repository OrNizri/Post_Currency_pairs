USE [Currencies]
GO
SET ANSI_NULLS ON
GO

create procedure Proc_Set_Currency_Pairs (
	@currency_pair varchar(100),
	@currency_pair_value float,
	@currency_pair_time datetime

) 

AS
BEGIN

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

INSERT into [CurrenciesPairs] ([currency_pair],[currency_pair_value],[currency_pair_time])
VALUES
(@currency_pair,@currency_pair_value,@currency_pair_time)

end;


