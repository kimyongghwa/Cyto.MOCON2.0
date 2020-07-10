var io = require('socket.io')({
	transports: ['websocket'],
});

io.attach(4445);

console.log('SERVER ON')

//socketio room
var room = ['room1','room2','room3','room4','room5','room6','room7','room8','room9','room10']

//room에 있는 사람 수
var CK = [0,0,0,0,0,0,0,0,0,0]

//room이                게임 시작했으면 1            게임 시작하지 않았으면 0
var playing = [0,0,0,0,0,0,0,0,0,0]
var key = 0


io.on('connection', socket=>{
	
	socket.on("joinRoom",data=> {

		id = data['sid']

		socket.join(room[key], () => {
			socket.emit('joinRoom',{'key':key})
			console.log(id +' join ' + room[key]+' key '+key);
			CK[key]+=1
			
			if(CK[key]==2 && !playing[key]){ //방에 2명 있으면 StartCyto
				io.sockets.in(room[key]).emit('StartCyto')
				playing[key]=1
				console.log('StartCyto key : '+key)
			}
				
			for(var i=0; i<10; i++){ //게임중이 아니고 인원이 2명이 아니면
				if(CK[i]!=2 && !playing[i]){
					key=i
					break
				}
			}
			
		});
		

	})

	socket.on('leaveRoom', data => { //메인화면으로 나가기 버튼
		keydata = data['key']
		id = data['id']
		console.log('leaveRoomDATA '+keydata)
		socket.leave(room[keydata], () => {
			console.log(id+' leave ' + room[keydata]);
			//상대한테 상대가 떠났습니다 보내야 함 
			socket.broadcast.to(room[keydata]).emit('OpponentLeft',data) 
			CK[keydata]-=2
			playing[keydata]=0
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



