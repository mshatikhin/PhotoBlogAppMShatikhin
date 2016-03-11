// В этом примере мы также имеем два компонента - изображение и список изображений
// Изображения получены из Instagram через AJAX.


var Picture = React.createClass({

    // Этот компонент не содержит никакого состояния - он просто преобразует
    // то что было передано атрибутами в изображение.

    clickHandler: function(){
        
        // Когда компонент кликнут, вызываем обработчик onClick, 
        // который был передан атрибутом при создании:

        this.props.onClick(this.props.ref);
    },

    render: function(){

        var cls = 'picture ' + (this.props.favorite ? 'favorite' : '');

        return (

            <div className={cls} onClick={this.clickHandler}>
                <img src={this.props.src} width="200" title={this.props.title} />
            </div>

        );

    }

});

var PictureList = React.createClass({

    getInitialState: function(){
        
        // Массив изображений будет передан по AJAX, а 
        // избранные когда, пользователь кликнет по изображению:
        
        return { pictures: [], favorites: [] };
    },

    componentDidMount: function(){
        
        // Когда компонент загружается,  отправляем jQuery AJAX запрос

        var self = this;

        // конечная точка API, для загрузки популярных изображений дня

        var url = 'https://api.instagram.com/v1/media/popular?client_id=' + this.props.apiKey + '&callback=?';

        $.getJSON(url, function(result){

            if(!result || !result.data || !result.data.length){
                return;
            }

            var pictures = result.data.map(function(p){

                return { 
                    id: p.id, 
                    url: p.link, 
                    src: p.images.low_resolution.url, 
                    title: p.caption ? p.caption.text : '', 
                    favorite: false 
                };

            });

            // Обновляем состояние компонента. Это вызовет render.
            // Заметьте, что это обновляет только свойство pictures
            // и не удаляет массив избранных.

            self.setState({ pictures: pictures });

        });

    },

    pictureClick: function(id){

        // id содержит ID кликнутого изображения.
        // Найдем в массиве pictures и добавим его в избранные

        var favorites = this.state.favorites,
            pictures = this.state.pictures;

        for(var i = 0; i < pictures.length; i++){

            // Находим айди в массиве изображений

            if(pictures[i].id == id) {                  

                if(pictures[i].favorite){
                    return this.favoriteClick(id);
                }

                // Добавляем изображение в массив избранных,
                // и отмечаем, как избранное:

                favorites.push(pictures[i]);
                pictures[i].favorite = true;

                break;
            }

        }

        // Обновляем состояние, вызывая перерисовку
        this.setState({pictures: pictures, favorites: favorites});

    },

    favoriteClick: function(id){

        // Находим изображение в списке избранных и удалаяем его 
        // После этого находим изображение в массиве всех изображений и отмечаем, как не-избранное.

        var favorites = this.state.favorites,
            pictures = this.state.pictures;


        for(var i = 0; i < favorites.length; i++){
            if(favorites[i].id == id) break;
        }

        // Удаляем из избранных
        favorites.splice(i, 1);


        for(i = 0; i < pictures.length; i++){
            if(pictures[i].id == id) {
                pictures[i].favorite = false;
                break;
            }
        }

        // Обновляем состояние и перерисовываем
        this.setState({pictures: pictures, favorites: favorites});

    },

    render: function() {

        var self = this;

        var pictures = this.state.pictures.map(function(p){
            return <Picture ref={p.id} src={p.src} title={p.title} favorite={p.favorite} onClick={self.pictureClick} />
        });

        if(!pictures.length){
            pictures = <p>Loading images..</p>;
        }

        var favorites = this.state.favorites.map(function(p){
            return <Picture ref={p.id} src={p.src} title={p.title} favorite={true} onClick={self.favoriteClick} />
        });

        if(!favorites.length){
            favorites = <p>Click an image to mark it as a favorite.</p>;
        }

        return (

            <div>
                <h1>Popular Instagram pics</h1>
                <div className="pictures"> {pictures} </div>
                    
                <h1>Your favorites</h1>
                <div className="favorites"> {favorites} </div>
            </div>

        );
    }
});


// Отрисовываем компонент PictureList и добавлем его в body.
// я использую API ключ для тестового Instagram приложения.  
// Пожалуйста, сгенерируйте и используйте свой собственный здесь http://instagram.com/developer/

React.render(
    <PictureList apiKey="642176ece1e7445e99244cec26f4de1f" />,
    $(".pictures")[0]
);