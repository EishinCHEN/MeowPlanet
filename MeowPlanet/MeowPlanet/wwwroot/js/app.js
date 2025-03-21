const navSildes =()=>{
    const burgers = document.querySelector('.burger');
    const navs = document.querySelector('.links');
    const navLink = document.querySelectorAll('.links li')

    burgers.addEventListener('click',()=>{
        navs.classList.toggle('nav-active');


        navLink.forEach((link,index)=>{
            if(link.style.animation){
                link.style.animation='';
            }else{
                link.style.animation=`navLinkFade 0.5s ease forwards ${index / 7 + 0.3}s`;
            }
        });

        burgers.classList.toggle('toggle');
    });

    
}

navSildes();
