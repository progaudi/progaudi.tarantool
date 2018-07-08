package main

import (
	"fmt"
	"log"
	"time"

	"github.com/tarantool/go-tarantool"
)

func main() {
	opts := tarantool.Opts{}
	conn, err := tarantool.Connect("tarantool_1_8:3301", opts)

	// conn, err := tarantool.Connect("/path/to/tarantool.socket", opts)
	if err != nil {
		fmt.Println("Connection refused: %s", err.Error())
	}
	start := time.Now()
	f := make([]*tarantool.Future, 0)
	for i := 0; i < 1000000; i++ {
		fut := conn.InsertAsync("pivot", []interface{}{i, []int{i, i}, i})
		f = append(f, fut)
	}
	for _, element := range f {
		_, err := element.Get()
		if err != nil {
			fmt.Println("Insert failed: %s", err.Error())
		}
	}
	elapsed := time.Since(start)
	log.Printf("Insert took %s", elapsed)
}
