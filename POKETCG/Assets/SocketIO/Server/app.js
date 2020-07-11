var io = require('socket.io')({
	transports: ['websocket'],
});

io.attach(4445);

console.log('SERVER ON')

//socketio room
var room = ['room1','room2','room3','room4','room5','room6','room7','room8','room9','room10'
,'room11','room12','room13','room14','room15','room16','room17','room18','room19'
,'room20','room21','room22','room23','room24','room25','room26','room27','room28'
,'room29','room30','room31','room32','room33','room34','room35','room36','room37'
]

//room에 있는 사람 수
var CK = [0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0]

//room                게임 시작했으면 1            게임 시작하지 않았으면 0
var playing = [0,0,0,0,0,0,0,0,0,0]
var key


io.on('connection', socket=>{
	
	socket.on("joinRoom",data=> {

		id = data['sid']

		for(var i=0; i<10; i++){ //게임중이 아니고 인원이 2명이 아니면
			if(CK[i]!=2 && !playing[i]){
				key=i
				break
			}
		}

		socket.join(room[key], () => {
			socket.emit('joinRoom',{'key':key})
			console.log(id +' join ' + room[key]+' key '+key);
			CK[key]+=1
			
			if(CK[key]==2 && !playing[key]){ //방에 2명 있으면 StartCyto
				io.sockets.in(room[key]).emit('StartCyto')
				// socket.emit('StartRoop') //2번째 플레이에게 roop를 보냄
				playing[key]=1
				console.log('StartCyto key : '+key)
			}
			
		});
		
	})

	socket.on('leaveRoom', data => { //메인화면으로 나가기 버튼
		keydata = data['key']
		id = data['id']
		console.log('leaveRoomDATA '+keydata)
		socket.leave(room[keydata],()=>{})
		socket.leave(room[keydata], () => {
			console.log(id+' leave ' + room[keydata]);
			//상대한테 상대가 떠났습니다 보내야 함 
			socket.broadcast.to(room[keydata]).emit('OpponentLeft') 
			CK[keydata]-=2
			playing[keydata]=0
			console.log(CK[key])

		});
	});

	socket.on('EndCyto', data=>{ //게임 승패가 갈린 경우
		keydata = data['key']
		id = data['id']
		console.log('EndCyto '+keydata)
		socket.leave(room[keydata], () => { 
			console.log(id+' leave ' + room[keydata]);
			CK[keydata]-=1
			playing[keydata]=0
		});
	})

	// socket.on('Alive',data=>{
	// 	keydata = data['key']
	// 	id = data['id']
	// 	//socket.broadcast.to(room[keydata]).emit('Alive',data)
	// 	console.log(id+' alive!') 
	// })

	socket.on('CanceljoinRoom', data => { //대기화면 취소버튼
		keydata = data['key']
		id = data['id']
		console.log('CanceljoinRoom '+keydata)
		socket.leave(room[keydata], () => {
			console.log(id+' cancel ' + room[keydata]);
			CK[keydata]-=1
		});
	});


	// socket.on('ShutCyto', data=>{ // 한 명이 게임을 강제종료
	// 	keydata = data['key']
	// 	id = data['id']
	// 	console.log('ShutCyto '+keydata+" "+room[keydata])
	// 	socket.leave(room[keydata], () => { 
	// 		CK[keydata]-=1
	// 	});
	// 	socket.leave(room[keydata], () => { 
	// 		CK[keydata]-=1
	// 		playing[keydata]=0
	// 	});
	// })

	socket.on('disconnect', () => {
		console.log('a user disconnected');
	  });
	

	socket.on('MyCard',data=>{
		keydata = data['key']
		// console.log('-------MyCard--------')
		// console.log(data)
		socket.broadcast.to(room[keydata]).emit('OpponentCard',data) 
	})


	socket.on('MyCharacter',data=>{
		keydata = data['key']
		// console.log('-----------MyCharacter--------')
		// console.log(data)
		socket.broadcast.to(room[keydata]).emit('OpponentCharacter',data) 
	})

	socket.on('MySkill',data=>{
		keydata = data['key']
		// console.log('---------MySkill--------')
		// console.log(data)
		socket.broadcast.to(room[keydata]).emit('OpponentSkill',data) 
	})

	socket.on('MyCheck',data=>{
		keydata = data['key']
		// console.log('--------MyCheck--------')
		// console.log(data)
		socket.broadcast.to(room[keydata]).emit('OpponentCheck',data) 
	})
})



