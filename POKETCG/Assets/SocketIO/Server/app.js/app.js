//const { debug } = require('console');

var io = require('socket.io')({
	transports: ['websocket'],
});

io.attach(4445);
console.log('SERVER ON')
let room = ['room1','room2','room3','room4','room5','room6','room7','room8','room9','room10'];
let CK = [0,0,0,0,0,0,0,0,0,0];	
let key=0

// var userlist = { room에서 이상이 생길 때 socket.sid랑 같이 고쳐볼 수도 있음
// 	users: [],
// 	createUser(id) {
// 		this.users.push({
// 		id
// 		})
// 	},
// 	deleteUser(id) {
// 		let idx = this.users.findIndex(x => x.id == id)
// 		if (idx != -1)
// 		this.users.splice(idx, 1)
// 	},
// 	has(id) {
// 		console.log(this.users.findIndex(x => x.id == id))
// 		return this.users.findIndex(x => x.id == id)
		
//     }
// }
io.on('connection', socket=>{
	
	socket.on("joinRoom",data=> {
		id = data['sid']
		// if (userlist.has(id) == -1 && id != '') {
		// 	userlist.createUser(id)
		// 	// 앞에서 둘씩 잘라서 room에 넣음
		// 	console.log(userlist.users[0])
		// 	console.log(userlist.users[1])
		// 	console.log(userlist.users[2])
		// 	console.log(userlist.users[3])
		// }
		//userlist.createUser(id)
		//console.log(userlist.users[0])
		//console.log(userlist.users[1])
		//console.log(userlist.users[2])
		//console.log(userlist.users[3])
		// 앞에서 둘씩 잘라서 room에 넣음
		socket.join(room[key], () => {
			console.log(id +' join ' + room[key]+' key '+key);
			//io.to(room[key]).emit('joinRoom',{'key':key}); room 전체
			//socket.broadcast.to(room[key]).emit('joinRoom',{'key':key}); room에 나 뺀 애들한테
			socket.emit('joinRoom',{'key':key})
		});
		for(var i=0; i<10; i++){
			if(CK[i]!=2){
				key=i
				CK[key]+=1
				if(CK[i]==2){
					console.log('StartCyto key : '+key)
					io.to(room[key]).emit('StartCyto')
				}
				break
			}
		}

	})

	// socket.on("boomRoom", data => {
	// io.sockets.to(data.roomCode).emit("boomRoom", data)

	// })
	// socket.on("getMsg", data => {
	// socket.broadcast.to(data.roomCode).emit("getMsg", data)
	// })

	// socket.on('disconnect', data => [
	// 	userlist.deleteUser(socket.id)
	// ])

	socket.on('leaveRoom', data => {
		keydata = data['key']
		id = data['id']
		console.log(data)
		CK[keydata]-=1
		socket.leave(room[keydata], () => {
			console.log(id+' leave ' + room[keydata]);
			//io.to(room[key]).emit('leaveRoom', {'key':key});
		});
	});

	socket.on('disconnect', () => {
		console.log('user disconnected');
	  });
	

	socket.on('MyCard',data=>{
		key = data['key']
		console.log('-------MyCard--------')
		console.log(data)
		socket.emit('OpponentCard',data)
		//socket.broadcast.to(room[key]).emit('OpponentCard',data) 
	})


	socket.on('MyCharacter',data=>{
		key = data['key']
		console.log('-----------MyCharacter--------')
		console.log(data)
		socket.emit('OpponentCharacter',data)
		//socket.broadcast.to(room[key]).emit('OpponentCharacter',data) 
	})

	socket.on('MySkill',data=>{
		key = data['key']
		console.log('---------MySkill--------')
		console.log(data)
		socket.emit('OpponentSkill',data)
		//socket.broadcast.to(room[key]).emit('OpponentSkill',data) 
	})
	socket.on('MyCheck',data=>{
		key = data['key']
		console.log('--------MyCheck--------')
		console.log(data)
		socket.emit('OpponentCheck',data)
		//socket.broadcast.to(room[key]).emit('OpponentCheck',data) 
	})
})



