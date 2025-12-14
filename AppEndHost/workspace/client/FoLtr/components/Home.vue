<template>
    <div>
        <div key="hero-wrapper">
            <component-loader src="components/Hero" uid="heroSection" cid="heroSection" />
        </div>
       
        <div key="features-wrapper">
            <component-loader src="components/Features" uid="featuresSection" cid="featuresSection" />
        </div>
         <div key="stats-wrapper">
            <component-loader src="components/Stats" uid="statsSection" cid="statsSection" />
        </div>
        <div key="products-wrapper">
            <component-loader src="components/ProductsSection" uid="productsSection" cid="productsSection" />
        </div>
        <div key="about-wrapper">
            <component-loader src="components/AboutUs" uid="aboutSection" cid="aboutSection" />
        </div>
        <div key="pricing-wrapper">
            <component-loader src="components/Pricing" uid="pricingSection" cid="pricingSection" />
        </div>
        <div key="testimonials-wrapper">
            <component-loader src="components/Testimonials" uid="testimonialsSection" cid="testimonialsSection" />
        </div>
        <div key="blog-wrapper">
            <component-loader src="components/BlogPreview" uid="blogSection" cid="blogSection" />
        </div>
        <div key="contact-wrapper">
            <component-loader src="components/ContactForm" uid="contactSection" cid="contactSection" />
        </div>
        <div key="footer-wrapper">
            <component-loader src="components/Footer" uid="footerSection" cid="footerSection" />
        </div>
    </div>
</template>

<script>
let _this = { cid: "", c: null };

export default {
    setup(props) { 
        _this.cid = props['cid']; 
    },
    data() { 
        return _this; 
    },
    created() { 
        _this.c = this;
        console.log('[Home] Component created');
    },
    mounted() {
        console.log('[Home] Component mounted');
        console.log('[Home] Checking component loaders in template...');
        
        // Check Vue component loaders in the template
        const allLoaders = this.$el.querySelectorAll('component-loader');
        console.log(`[Home] Found ${allLoaders.length} component-loader elements in template`);
        allLoaders.forEach((loader, index) => {
            const src = loader.getAttribute('src');
            const cid = loader.getAttribute('cid');
            console.log(`[Home] Loader ${index}: src="${src}", cid="${cid}"`);
        });
        
        // Check all component loaders
        const componentNames = ['heroSection', 'statsSection', 'featuresSection', 'productsSection', 
                               'aboutSection', 'pricingSection', 'testimonialsSection', 'blogSection', 
                               'contactSection', 'footerSection'];
        
        setTimeout(() => {
            console.log('[Home] Checking loaded components after 2 seconds...');
            componentNames.forEach(cid => {
                const loader = document.querySelector(`[cid="${cid}"]`);
                const component = document.querySelector(`#${cid}`);
                console.log(`[Home] Component "${cid}":`, {
                    loaderExists: !!loader,
                    componentExists: !!component,
                    loaderElement: loader,
                    componentElement: component
                });
            });
            
            // Check for any errors
            const allSections = document.querySelectorAll('.section');
            console.log(`[Home] Found ${allSections.length} sections in DOM`);
            allSections.forEach((section, index) => {
                console.log(`[Home] Section ${index}:`, {
                    id: section.id,
                    className: section.className,
                    visible: section.offsetHeight > 0
                });
            });
            
            // Check network requests
            console.log('[Home] Checking Network tab for failed requests...');
        }, 2000);
        
        // Also check after 5 seconds
        setTimeout(() => {
            console.log('[Home] Final check after 5 seconds...');
            componentNames.forEach(cid => {
                const component = document.querySelector(`#${cid}`);
                if (!component) {
                    console.warn(`[Home] WARNING: Component "${cid}" still not loaded after 5 seconds!`);
                }
            });
        }, 5000);
        
        // Scroll animations
        const observerOptions = {
            threshold: 0.1,
            rootMargin: '0px 0px -50px 0px'
        };
        
        const observer = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    entry.target.classList.add('fade-in-up');
                }
            });
        }, observerOptions);
        
        document.querySelectorAll('.section').forEach(section => {
            observer.observe(section);
        });
        
    },
    props: { cid: String }
}
</script>

<style scoped>
</style>

