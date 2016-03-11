﻿var MenuExample = React.createClass({

    getInitialState: function(){
        return { focused: 0 };
    },       

    clicked: function(index){

        // Обработчик клика обновит состояние
        // изменив индекс на сфокусированный элемент меню

        this.setState({focused: index});
    },

    render: function() {

        // Здесь мы читаем свойство items, которое было передано
        // атрибутом, при создании компонента

        var self = this;

        // Метод map пройдется по массиву элементов меню,
        // и возвратит массив с <li> элементами.

        return (
            <div>
                <ul>{ this.props.items.map(function(m, index){
        
                    var style = '';
        
                    if(self.state.focused == index){
                        style = 'focused';
                    }
        
                    // Обратите внимание на использование метода bind(). Он делает
                    // index доступным в функции clicked:
        
                    return <li key={index} className={style} onClick={self.clicked.bind(self, index)}>{m}</li>;
        
                }) }
                        
                </ul>
                
                <p>Selected: {this.props.items[this.state.focused]}</p>
            </div>
        );

    }
});

// Отображаем компонент меню на странице, передав ему массив с элементами

React.render(
    <MenuExample items={ ['Home', 'Services', 'About', 'Contact us'] } />,
    $(".menunav")[0]
);