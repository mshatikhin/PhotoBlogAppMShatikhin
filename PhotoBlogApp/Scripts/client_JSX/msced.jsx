
var Image = React.createClass({
    render: function(){
        return (
            <div className="msced__image">
                <img src={this.props.src} title={this.props.title} />
            </div>
        );
    }
});

var ImageGallery = React.createClass({
	loadPhotosFromServer: function() {
		var self = this;
		var nextPage = this.state.page+1;

		$.getJSON(this.props.url+"?page="+this.state.page, function(result) {		  
		  if(result == null){
               return;
          }
		  var images = result.map(function(p){

                return { 
                    id: p.PhotoId, 
                    url: p.Title, 
                    src: p.FullUrl, 
                    title: p.Title
                };

          });	  

		  self.setState({ images: self.state.images.concat(images), page: nextPage});
		});
	},
	handleScroll: function(e){
		var windowHeight = $(window).height();
		var inHeight = window.innerHeight;
		var scrollT = $(window).scrollTop();
		var totalScrolled = scrollT+inHeight;
		if(totalScrolled+100>windowHeight){  //user reached at bottom
		  //if(!this.state.loadingFlag){  //to avoid multiple request 
			//  
			//  loadPhotosFromServer.getPhotos();
		  //}

		  this.loadPhotosFromServer();
		}
  	},
	getInitialState: function() {		
		$(window).scroll(this.handleScroll);
		return {images: [], page:0 };
	},
	componentWillMount: function() {
		this.loadPhotosFromServer();
		//window.setInterval(this.loadPhotosFromServer, this.props.pollInterval);
    },
    render: function ()
    {
		var images = this.state.images.map(function(p){
            return <Image src={p.src} title={p.title} />
        });

        return (
            <div className="msced__image-gallery">
				{images}
			</div>
        );
	}
});


React.render(
	<ImageGallery url="/msced/GetPhotos" pollInterval={2000}/>, $(".msced")[0]
);