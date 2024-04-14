package model

type Commands struct {
	Turn  string
	Stand string
	Walk  string
	Jump  string
	Other string
}

var Standard_commands Commands = Commands{
	"turned", "stood", "walked", "jumped", "",
}
